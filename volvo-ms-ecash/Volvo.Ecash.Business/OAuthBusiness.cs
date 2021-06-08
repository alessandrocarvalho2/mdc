using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Volvo.Ecash.Application.Utils;
using Volvo.Ecash.Business.IBusiness;
using Volvo.Ecash.Domain.Entities;
using Volvo.Ecash.Domain.Filters;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository;
using static Volvo.Ecash.Dto.Enum.EnumCommon;

namespace Volvo.Ecash.Business
{
    public class OAuthBusiness : BaseBusiness<UnitOfWork>
    {
        public OAuthBusiness() : base(new ECashContext())
        {
        }

        /// <summary>
        /// Constructor of class"/>.
        /// </summary>
        /// <param name="Parent"></param>
        public OAuthBusiness(BaseBusiness<UnitOfWork> Parent) : base(Parent)
        {
        }

        /// <summary>
        /// Endpoint to authenticate in the system and receive a JWT format token, where there will be an expiration date
        /// </summary>
        /// <param name="credenciais">Access Credentials</param>
        /// <param name="loginBusiness"></param>
        /// <param name="signingConfigurations"></param>
        /// <param name="tokenConfigurations"></param>
        /// <param name="cache"></param>
        /// <returns>Token object, Refresh Token and expiration date</returns>
        public object Post(
             [FromBody] AccessCredentials credenciais,
             [FromServices] ILoginBusiness loginBusiness,
             [FromServices] SigningConfigurations signingConfigurations,
             [FromServices] TokenConfigurations tokenConfigurations,
             [FromServices] IDistributedCache cache)
        {

            try
            {
                UserVR userBase = null;
                bool credenciaisValidas = false;
                if (credenciais != null)
                {
                    if (credenciais.GrantType == "password" && !String.IsNullOrWhiteSpace(credenciais.UserID))
                    {
                        userBase = loginBusiness.Find(credenciais.UserID, credenciais.AccessKey).Items[0];
                        credenciaisValidas = (userBase != null &&
                            credenciais.UserID == userBase.UserID &&
                            credenciais.AccessKey == userBase.AccessKey);
                    }
                    else if (credenciais.GrantType == "refresh_token")
                    {
                        if (!String.IsNullOrWhiteSpace(credenciais.RefreshToken))
                        {
                            RefreshTokenData refreshTokenBase = null;

                            string strTokenArmazenado = cache.GetString(credenciais.RefreshToken);
                            if (!String.IsNullOrWhiteSpace(strTokenArmazenado))
                            {
                                refreshTokenBase = JsonConvert.DeserializeObject<RefreshTokenData>(strTokenArmazenado);
                                userBase = loginBusiness.Find(refreshTokenBase.UserID, refreshTokenBase.AccessKey);
                            }

                            credenciaisValidas = (refreshTokenBase != null &&
                                userBase.UserID == refreshTokenBase.UserID &&
                                credenciais.RefreshToken == refreshTokenBase.RefreshToken);

                            // Elimina o token de refresh já que um novo será gerado
                            if (credenciaisValidas)
                                cache.Remove(credenciais.RefreshToken);
                        }

                    }
                }

                if (credenciaisValidas)
                {
                    return GenerateToken(userBase,
                                         signingConfigurations,
                                         tokenConfigurations,
                                         cache);
                }
                else
                {
                    return new
                    {
                        authenticated = false,
                        message = "Falha ao autenticar"
                    };
                }

            }
            catch (Exception ex)
            {
                return new
                {
                    authenticated = false,
                    message = ex.Message
                };
            }

        }

        private object GenerateToken(UserVR user,
            SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations,
            IDistributedCache cache)
        {

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserID));

            //user.Roles.ForEach(x =>
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, x));
            //});

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.UserID, "Login"),
                claims.ToArray()
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            // Calculates the maximum validity time of the refresh token (it will be automatically invalidated by Redis)
            TimeSpan finalExpiration =
                TimeSpan.FromSeconds(tokenConfigurations.FinalExpiration);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });

            var token = handler.WriteToken(securityToken);

            var resultado = new
            {
                authenticated = true,
                created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
                refreshToken = Guid.NewGuid().ToString().Replace("-", String.Empty),
                message = "OK"
            };

            // Caches the refresh token through Redis
            var refreshTokenData = new RefreshTokenData()
            {
                RefreshToken = resultado.refreshToken,
                UserID = user.UserID,
                AccessKey = user.AccessKey
            };

            //DistributedCacheEntryOptions opcoesCache =
            //    new DistributedCacheEntryOptions();
            //opcoesCache.SetAbsoluteExpiration(finalExpiration);
            //cache.SetString(resultado.refreshToken,
            //    JsonConvert.SerializeObject(refreshTokenData),
            //    opcoesCache);

            return resultado;
        }
    }

    public class LoginBusiness : ILoginBusiness
    {
        public dynamic Find(string userID, string accessKey)
        {
            try
            {
                using (var business = new UserBusiness())
                {
                    var resultModel = new ResultModel<User>(true);

                    var user = business.GetList(new UserFilter()
                    {
                        Login = userID,
                        Password = accessKey

                    }).Items.FirstOrDefault();

                    if (user != null)
                    {
                        if (user.Active)
                        {
                            resultModel.Items.Add(new User()
                            {
                                UserID = userID.ToString(),
                                UserName = userID.ToString(),
                                FirstName = user.Name,
                                FullName = user.Name,
                                Email = user.Email,
                                AccessKey = accessKey,
                                Password = user.Password,
                                CreateTimeStamp = DateTime.Now.ToString(),
                            });

                            resultModel.Pages = null;
                            resultModel.AddMessage(string.Format(SuccessMessage.MSG006), SystemMessageTypeEnum.Success);
                            return resultModel;
                        }
                        else
                        {
                            resultModel.IsOk = false;
                            resultModel.AddMessage(string.Format(ErrorMessage.MSG021, "Usuário"), SystemMessageTypeEnum.Info);
                        }
                    }
                    else
                    {
                        resultModel.IsOk = false;
                        resultModel.AddMessage(ErrorMessage.MSG022, SystemMessageTypeEnum.Info);
                    }

                    return resultModel;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
