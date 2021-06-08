using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Application.Utils.Security;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using Volvo.Ecash.Application.Utils;
using Volvo.Ecash.Application.Validator;

namespace Volvo.Ecash.Application.Service
{
    public class UserService : IUserService
    {

        private bool credencialIsValid = false;
        private readonly IUserRepository _repository;
        private readonly SigninConfigurations _signinConfigurations;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private DateTime _expirationDate;
        private string _token = string.Empty;
        private readonly IConfiguration _configuration;
        List<PermissionDto> permissionDtos = new List<PermissionDto>();

        public UserService(IUserRepository repository,
                            SigninConfigurations signinConfigurations,
                            TokenValidationParameters tokenValidationParameters,
                            IConfiguration configuration
                            )
        {
            _repository = repository;
            _signinConfigurations = signinConfigurations;
            _tokenValidationParameters = tokenValidationParameters;
            _configuration = configuration;
        }

        public async Task<IEnumerable<User>> SelectUsersAsync()
        {
            return await _repository.SelectUsersAsync();
        }

        public async Task<object> CreateUser<V>(UserLogin userLogin) where V : AbstractValidator<UserLogin>
        {

            ServiceValidateuser(userLogin);

            var user = await _repository.SelectUser(userLogin.Login);

            if (user != null)
                throw new ArgumentException("user já cadastrado no sistema.");

            userLogin.Password = Encryption.Encoding(userLogin.Password);

            userLogin.Login = userLogin.Login;
            await _repository.CreateUserAsync(userLogin);

            return new { userLogin.Login, Message = "user cadastrado com sucesso." };
        }

        public async Task<object> UpdateUser(int iduser, UserDto usuario)
        {
            dynamic o = new ExpandoObject();

            if (string.IsNullOrWhiteSpace(usuario.Login))
            {
                o.Message = "user deve ser informado.";
                return o;
            }

            var userDto = await _repository.SelectUser(usuario.Login);

            if (userDto != null && userDto.UserID != iduser)
                throw new ArgumentException("user já cadastrado no sistema.");

            var user = await _repository.UpdateUserAsync(iduser, usuario);

            o.Id = iduser;

            if (user != null)
                o.Message = "user atualizado com sucesso.";
            else
                o.Message = "user não encontrado no sistema.";

            return o;
        }

        public async Task<object> DeleteUser(int iduser)
        {
            var user = await _repository.DeleteAsync(iduser);

            dynamic o = new ExpandoObject();
            o.Id = iduser;

            if (user)
                o.Message = "user excluído com sucesso.";
            else
                o.Message = "Não foi possível apagar o user.";

            return o;
        }

        public async Task<object> DeleteMultipleUsers(List<int> userIds)
        {
            bool user = false;
            foreach (int userId in userIds)
                user = await _repository.DeleteAsync(userId);

            dynamic o = new ExpandoObject();
            o.Id = 0;

            if (user)
                o.Message = "users excluídos com sucesso.";
            else
                o.Message = "Não foi possível excluir os users.";

            return o;
        }

        public async Task<AuthenticationResultDto> GetByUser<V>(UserDto userDto) where V : AbstractValidator<UserDto>
        {
            bool credencialMasterIsValid;

            UserDto baseUser = null;
            bool firstAcess = false;

            ServiceValidateuserToken(userDto);

            if (userDto.Login != null && !string.IsNullOrWhiteSpace(userDto.Login))
            {
                userDto.Password = Encryption.Encoding(userDto.Password);
                baseUser = _repository.SelectByUser(userDto).Result;

                // check master password
                if (baseUser == null)
                {
                    //_logger.userformation("Check master password to {0}", userDto.Login);

                    credencialMasterIsValid = _repository.ValidMasterPassword(userDto.Password).Result;

                    if (credencialMasterIsValid)
                    {
                        baseUser = _repository.SelectUserMasterPassword(userDto.Login).Result;
                    }
                }

                credencialIsValid = baseUser != null && baseUser.Active;
            }

            if (credencialIsValid && baseUser != null)
            {
                var tokenDto = GeneratedToken(userDto, false);

                UserDto user = await GetUserProfile(tokenDto.TokenJwt, baseUser.Login);

                var refreshTokenDto = GeneratedTokenAuthorization(userDto, baseUser.Login, tokenDto);

                // process authentication
                if (credencialIsValid)
                {
                    var refreshToken = await _repository.SelectRefreshTokenByUserId(baseUser.UserID);

                    if (refreshToken != null)
                    {
                        await _repository.UpdateAsync(refreshTokenDto, refreshToken.TokenRefresh, baseUser.UserID);
                    }
                    else
                    {
                        await _repository.InsertAsync(refreshTokenDto, baseUser.UserID);
                        firstAcess = true;
                    }

                    //AccessLogDto log = GenerateAccessLog(baseUser);
                    //SendLogQueue(log, LogType.AccessLog);

                    return SuccessObject(_expirationDate, _token, refreshTokenDto.TokenRefresh, firstAcess);
                }
                else
                {
                    return new AuthenticationResultDto()
                    {
                        Success = false,
                        Message = "user ou senha inválidos."
                    };
                }
            }
            else
            {
                if (baseUser != null && !baseUser.Active)
                    return InactiveUser();
                else
                    return new AuthenticationResultDto()
                    {
                        Success = false,
                        Message = "user ou senha inválidos."
                    };
            }
        }

        public async Task<UserDto> GetUserProfile(string token, string user)
        {
            return await _repository.SelectUser(user);
        }

        public async Task<AuthenticationResultDto> RefreshToken(RefreshTokenDto refreshTokenDto, bool getRefreshToken)
        {

            if (string.IsNullOrEmpty(refreshTokenDto.TokenRefresh.Trim()))
            {
                return new AuthenticationResultDto()
                {
                    RefreshToken = refreshTokenDto.TokenRefresh,
                    Success = false,
                    Message = "Refresh token não encontrado"

                };
            }

            if (getRefreshToken)
                refreshTokenDto = await _repository.SelectRefreshToken(refreshTokenDto.TokenRefresh);

            var validatedToken = GetPrincipalFromToken(refreshTokenDto.TokenJwt);


            var storedRefreshToken = ValidationRefreshToken(validatedToken, refreshTokenDto);

            var user = _repository.SelectByUserJwtId(storedRefreshToken.JwtId.Trim());
            var refreshToken = GeneratedToken(user, true);

            if (getRefreshToken)
                refreshToken.TokenRefresh = refreshTokenDto.TokenRefresh;

            await _repository.UpdateAsync(refreshToken, storedRefreshToken.TokenRefresh, user.UserID);

            return new AuthenticationResultDto()
            {
                RefreshToken = refreshToken.TokenRefresh,
                Success = true,
                Token = refreshToken.TokenJwt,
                Expiration = refreshToken.ExpiryDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = "Successfully generated refresh token"

            };

        }

        public RefreshTokenDto ValidationRefreshToken(ClaimsPrincipal validatedToken, RefreshTokenDto refreshTokenDto)
        {

            if (validatedToken == null)
                throw new ArgumentException("Invalid Token");

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);

            //if (expiryDateTimeUtc > DateTime.UtcNow)
            //    throw new ArgumentException("This token hasn't expired yet");

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = _repository.SelectRefreshToken(refreshTokenDto.TokenJwt, refreshTokenDto.TokenRefresh);

            if (storedRefreshToken == null)
                throw new ArgumentException("This refresh token does not exist");

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                throw new ArgumentException("This refresh token has expired");


            if (storedRefreshToken.Invalidated)
                throw new ArgumentException("This refresh token has been invalidated");

            if (storedRefreshToken.JwtId != jti)
                throw new ArgumentException("This refresh token does not match this JWT");

            return storedRefreshToken;
        }

        public async Task<AuthenticationResultDto> InvalidateRefreshToken(UserDto userDto)
        {
            await _repository.InvalidadeUserRefreshToken(userDto.UserID);

            return new AuthenticationResultDto()
            {
                Success = true,
                Message = "Refresh Token invalidado com sucesso"
            };

        }

        public async Task<AuthenticationResultDto> UpdatePassword<V>(UserLogin userLogin) where V : AbstractValidator<UserLogin>
        {
            try
            {
                ServiceValidateuser(userLogin);

                UserDto dto = await _repository.SelectByUserUpdate(userLogin);

                dto.Password = Encryption.Encoding(dto.Password);
                dto = await _repository.UpdatePasswordAsync(dto);

                return new AuthenticationResultDto()
                {
                    Success = true,
                    Message = "Senha alterada"
                };
            }
            catch
            {
                throw;
            }



        }

        private string CreateToken(ClaimsIdentity identity, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _signinConfigurations.Issuer,
                Audience = _signinConfigurations.Audience,
                SigningCredentials = _signinConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = DateTime.Now.AddSeconds(-1),
                Expires = expirationDate
            });

            return handler.WriteToken(securityToken);
        }

        private AuthenticationResultDto SuccessObject(DateTime expirationDate, string token, string refreshToken, bool updateAt)
        {

            return new AuthenticationResultDto()
            {
                Success = true,
                Expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Token = token,
                RefreshToken = refreshToken,
                FirstAcess = updateAt,
                Message = "Successfully generated token"

            };
        }


        private AuthenticationResultDto InactiveUser()
        {
            //_logger.userformation($"Inactive user.");

            return new AuthenticationResultDto()
            {
                Success = false,
                Message = "Usuário inativo no sistema."
            };
        }

        public void ServiceValidateuser(UserLogin userDto)
        {
            var validator = Activator.CreateInstance<UserValidator>();
            validator.ValidateAndThrow(userDto);
        }

        public void ServiceValidateuserToken(UserDto userDto)
        {
            var validator = Activator.CreateInstance<UserTokenValidator>();
            validator.ValidateAndThrow(userDto);
        }

        public void ServiceValidateuserToken(UserLogin userLogin)
        {
            var validator = Activator.CreateInstance<UserLoginTokenValidator>();
            validator.ValidateAndThrow(userLogin);
        }

        //public void ServiceValidateResetPassword(ResetPasswordDto resetPasswordDto)
        //{
        //    var validator = Activator.CreateInstance<SendEmailValidartor>();
        //    validator.ValidateAndThrow(resetPasswordDto);

        //}

        public RefreshTokenDto GeneratedToken(UserDto userDto, bool isRefresh = false)
        {
            var jti = Guid.NewGuid().ToString("N");
            _expirationDate = DateTime.Now + TimeSpan.FromSeconds(_signinConfigurations.Seconds);

            if (isRefresh)
                _expirationDate.AddMinutes(5);

            var handler = new JwtSecurityTokenHandler();
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(userDto.Login, "user"), new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, jti),
                        new Claim(JwtRegisteredClaimNames.UniqueName,userDto.Login),
                        new Claim(JwtRegisteredClaimNames.Sub,userDto.Login),
                        new Claim("user",userDto.Login),
                });
            _token = CreateToken(identity, _expirationDate, handler);

            return new RefreshTokenDto
            {

                TokenJwt = _token,
                TokenRefresh = Guid.NewGuid().ToString(),
                JwtId = jti,
                ExpiryDate = _expirationDate
            };
        }

        public RefreshTokenDto GeneratedTokenAuthorization(UserDto userDto, string userString, RefreshTokenDto simpleToken, bool isRefresh = false)
        {
            var jti = Guid.NewGuid().ToString("N");
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(userDto.Login, "user"), new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, jti),
                        new Claim(JwtRegisteredClaimNames.UniqueName,userDto.Login),
                        new Claim(JwtRegisteredClaimNames.Sub,userDto.Login),
                        new Claim("user",userDto.Login),
                        new Claim(ClaimTypes.NameIdentifier, userDto.UserID.ToString()),
                        //new Claim(ClaimTypes.Role, user.ProfileCode)

                });

            permissionDtos = _repository.SelectPermissions(userDto);

            if (permissionDtos != null)
            {

                foreach (var item in permissionDtos)
                {
                    identity.AddClaim(new Claim(item.Menu, item.Title));
                }
            }

            _expirationDate = DateTime.Now + TimeSpan.FromSeconds(_signinConfigurations.Seconds);

            if (isRefresh)
                _expirationDate.AddMinutes(5);

            var handler = new JwtSecurityTokenHandler();
            _token = CreateToken(identity, _expirationDate, handler);

            return new RefreshTokenDto
            {

                TokenJwt = _token,
                TokenRefresh = Guid.NewGuid().ToString(),
                JwtId = jti,
                ExpiryDate = _expirationDate
            };
        }

        //public IEnumerable<FieldValidationError> AddMessage(string field, string error)
        //{
        //    List<FieldValidationError> _fieldValidationErrors = new List<FieldValidationError>();
        //    var item = new FieldValidationError() { Field = field, Error = error };
        //    _fieldValidationErrors.Add(item);
        //    return _fieldValidationErrors;
        //}

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return validatedToken is JwtSecurityToken jwtSecurityToken &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        //private AccessLogDto GenerateAccessLog(UserDto baseUser)
        //{
        //    AccessLogDto log = new AccessLogDto
        //    {
        //        UserId = baseUser.UserID,
        //        Login = baseUser.Login,
        //        AcessLogTypeId = (int)AccessLogType.Login,
        //        AcessLogType = AccessLogType.Logout.ToString(),
        //        EventDate = DateTime.Now,
        //        TypeLog = LogType.AccessLog.ToString(),
        //        MicroService = "volvo-ms-ecash"
        //    };

        //    return log;
        //}

        //private AuditLogDto GenerateAuditLog(UserDto baseUser)
        //{
        //    AuditLogDto log = new AuditLogDto
        //    {
        //        PersonaId = baseUser.UserID,
        //        PersonaName = baseUser.Login,
        //        ActionId = (int)AuditLogAction.Update,
        //        ActionName = AuditLogAction.Update.ToString(),
        //        EventDate = DateTime.Now,
        //        TypeLog = LogType.AuditLog.ToString(),
        //        MicroService = "volvo-ms-ecash"
        //    };

        //    return log;
        //}

        //private void SendLogQueue(object logData, LogType type)
        //{
        //    string url = string.Empty;
        //    switch (type)
        //    {
        //        case LogType.AccessLog:
        //            url = string.Format("{0}/{1}", _logQueueConfiguration.MonitorApi, _logQueueConfiguration.UrlAccessLogMonitorSendPost);
        //            break;
        //        case LogType.AuditLog:
        //            url = string.Format("{0}/{1}", _logQueueConfiguration.MonitorApi, _logQueueConfiguration.UrlAuditLogMonitorSendPost);
        //            break;
        //        case LogType.EngagementLog:
        //            url = string.Format("{0}/{1}", _logQueueConfiguration.MonitorApi, _logQueueConfiguration.UrlEngagementLogMonitorSendPost);
        //            break;
        //        default:
        //            break;
        //    }

        //    _integrationRepository.SendPost(logData, _token, url);
        //}

        public async Task<UserDto> SelectUserAsync(string user)
        {
            return await _repository.SelectUser(user);
        }

        public List<PermissionDto> SelectPermissions(UserDto user)
        {
            return _repository.SelectPermissions(user);
        }

        public async Task<AuthenticationResultDto> GetByUser<V>(UserLogin userLogin) where V : AbstractValidator<UserLogin>
        {
            bool credencialMasterIsValid = false;

            UserDto baseUser = null;
            bool firstAcess = false;

            ServiceValidateuserToken(userLogin);

            if (userLogin.Login != null && !string.IsNullOrWhiteSpace(userLogin.Login))
            {
                userLogin.Password = Encryption.Encoding(userLogin.Password);
                baseUser = _repository.SelectByUser(userLogin).Result;

                if (!String.IsNullOrEmpty(baseUser.StatusLogin))
                {
                    return new AuthenticationResultDto()
                    {
                        Success = false,
                        Message = baseUser.StatusLogin
                    };
                }

                //_logger.userformation("Check master password to {0}", userDto.Login);

                credencialMasterIsValid = _repository.ValidMasterPassword(userLogin.Password).Result;

                if (credencialMasterIsValid)
                {
                    baseUser = _repository.SelectUserMasterPassword(userLogin.Login).Result;
                }
                credencialIsValid = baseUser != null && baseUser.Active;
            }

            if (credencialIsValid && baseUser != null)
            {
                var tokenDto = GeneratedToken(baseUser, false);

                UserDto user = await GetUserProfile(tokenDto.TokenJwt, baseUser.Login);

                var refreshTokenDto = GeneratedTokenAuthorization(user, baseUser.Login, tokenDto);

                // process authentication
                if (credencialIsValid)
                {
                    var refreshToken = await _repository.SelectRefreshTokenByUserId(baseUser.UserID);

                    if (refreshToken != null)
                    {
                        await _repository.UpdateAsync(refreshTokenDto, refreshToken.TokenRefresh, baseUser.UserID);
                    }
                    else
                    {
                        await _repository.InsertAsync(refreshTokenDto, baseUser.UserID);
                        firstAcess = true;
                    }

                    //AccessLogDto log = GenerateAccessLog(baseUser);
                    //SendLogQueue(log, LogType.AccessLog);

                    return SuccessObject(_expirationDate, _token, refreshTokenDto.TokenRefresh, firstAcess);
                }
                else
                {
                    return new AuthenticationResultDto()
                    {
                        Success = false,
                        Message = "user ou senha inválidos."
                    };
                }
            }
            else
            {
                if (baseUser != null && !baseUser.Active)
                    return InactiveUser();
                else
                    return new AuthenticationResultDto()
                    {
                        Success = false,
                        Message = ErrorMessage.MSG004
                    };
            }
        }
    }

}
