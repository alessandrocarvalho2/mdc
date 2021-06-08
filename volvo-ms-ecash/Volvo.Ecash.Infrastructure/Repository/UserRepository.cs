using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Volvo.Ecash.Application.Utils;
using Volvo.Ecash.Domain.Entities;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Context;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;
        private readonly IMapper _mapper;

        public UserRepository(UserContext userContext, IMapper mapper)
        {
            _userContext = userContext;
            _mapper = mapper;
            _userContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<IEnumerable<User>> SelectUsersAsync()
        {
            var result = await _userContext.User.ToListAsync();
            return result;
        }

        public async Task<UserDto> SelectByUser(UserLogin userLogin)
        {
            var loginOK = await _userContext.User.FirstOrDefaultAsync(x => x.Login == userLogin.Login);
            if (loginOK == null)
            {
                return new UserDto()
                {
                    StatusLogin = ErrorMessage.MSG023
                };
            }
            if (loginOK.Password == userLogin.Password)
            {
                return _mapper.Map<UserDto>(loginOK);
            }
            else
            {
                return new UserDto()
                {
                    StatusLogin = ErrorMessage.MSG024
                };
            }
        }
        public async Task<UserDto> SelectByUserUpdate(UserLogin userLogin)
        {
            var loginOK = await _userContext.User.FirstOrDefaultAsync(x => x.Login == userLogin.Login);
            return _mapper.Map<UserDto>(loginOK);
        }


        public async Task<UserDto> SelectByUser(UserDto userDto)
        {
            var result = await _userContext.User.FirstOrDefaultAsync(x => x.Login == userDto.Login && x.Password == userDto.Password && x.Active == true);
            return _mapper.Map<UserDto>(result);
        }

        public async Task<List<PermissionDto>> GetPermissions(User user)
        {
            var result = await _userContext.Permission.Where(p => p.ProfileId == user.UserID).ToListAsync();
            return result;
        }

        public async Task<bool> ValidMasterPassword(string password)
        {
            var isValidPassword = await _userContext.User.FirstOrDefaultAsync(x => x.Password == password);

            if (isValidPassword == null)
            {
                return false;
            }

            return true;
        }

        public async Task<UserDto> SelectUser(string user)
        {
            var result = await _userContext.User.FirstOrDefaultAsync(x => x.Login == user && x.Active == true);
            return _mapper.Map<UserDto>(result);
        }

        public async Task<UserDto> SelectUserMasterPassword(string user)
        {
            var result = await _userContext.User.FirstOrDefaultAsync(x => x.Login == user);
            return _mapper.Map<UserDto>(result);
        }

        public async Task<UserDto> SelectUserById(int id)
        {
            var result = await _userContext.User.FirstOrDefaultAsync(x => x.UserID == id && x.Active == true);
            return _mapper.Map<UserDto>(result);
        }



        public RefreshTokenDto SelectRefreshToken(string token, string refreshToken)
        {
            var result = _userContext.RefreshToken.FirstOrDefault(c => c.TokenJwt == token && c.TokenRefresh == refreshToken);
            return _mapper.Map<RefreshTokenDto>(result);
        }

        public async Task<RefreshToken> SelectRefreshTokenByUserId(int userId)
        {
            var result = await _userContext.RefreshToken.FirstOrDefaultAsync(c => c.Id == userId);
            return _mapper.Map<RefreshToken>(result);
        }

        public async Task<RefreshTokenDto> SelectRefreshTokenById(int id)
        {
            var result = await _userContext.RefreshToken.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<RefreshTokenDto>(result);
        }

        public async Task<RefreshTokenDto> SelectRefreshToken(string refreshToken)
        {
            var result = await _userContext.RefreshToken.FirstOrDefaultAsync(c => c.TokenRefresh == refreshToken);
            return _mapper.Map<RefreshTokenDto>(result);
        }

        public async Task<RefreshTokenDto> InsertAsync(RefreshTokenDto item, int userId)
        {
            var user = await _userContext.User.FirstOrDefaultAsync(x => x.UserID == userId && x.Active == true);

            var refreshToken = _mapper.Map<RefreshToken>(item);
            refreshToken.CreateAt = DateTime.Now;
            refreshToken.ExpiryDate = DateTime.Now.AddHours(6);

            _userContext.Set<RefreshToken>().Add(refreshToken);

            user.RefreshTokenId = refreshToken.Id;
            user.RefreshToken = refreshToken;
            _userContext.Entry(user).State = EntityState.Modified;

            await _userContext.SaveChangesAsync();

            return item;
        }

        public async Task<UserDto> UpdatePasswordAsync(UserDto userDto)
        {
            var user = await _userContext.User.FirstOrDefaultAsync(x => x.Login == userDto.Login && x.Active == true);

            if (user == null)
                throw new ArgumentException("User not found");

            userDto.UserID = user.UserID;
            user.UpdateAt = DateTime.Now;
            user.Password = userDto.Password;
            _userContext.Entry(user).State = EntityState.Modified;
            await _userContext.SaveChangesAsync();

            return userDto;

        }

        public async Task<UserDto> UpdateUserAsync(int idUser, UserDto user)
        {
            var item = await _userContext.User.FirstOrDefaultAsync(x => x.UserID == idUser && x.Active == true);

            if (item == null)
                throw new ArgumentException("User não cadastrado no sistema");

            item.Active = user.Active;
            item.Login = user.Login;
            item.UpdateAt = DateTime.Now;
            _userContext.Entry(item).State = EntityState.Modified;
            await _userContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(item);
        }

        public async Task<bool> DeleteAsync(int idUser)
        {
            var user = await _userContext.User.FirstOrDefaultAsync(x => x.UserID == idUser && x.Active == true);

            if (user == null)
                throw new ArgumentException("User não cadastrado no sistema");

            user.Active = false;
            user.UpdateAt = DateTime.Now;
            _userContext.Entry(user).State = EntityState.Modified;
            await _userContext.SaveChangesAsync();

            return true;
        }

        public async Task<RefreshTokenDto> UpdateAsync(RefreshTokenDto tokenDto, string tokenRefresh, int userId)
        {
            var result = await _userContext.RefreshToken.FirstOrDefaultAsync(x => x.TokenRefresh == tokenRefresh);

            if (result == null)
                return null;

            var user = await _userContext.User.FirstOrDefaultAsync(x => x.UserID == userId && x.Active == true);

            var refresh = _mapper.Map<RefreshToken>(tokenDto);
            refresh.Id = result.Id;
            refresh.CreateAt = result.CreateAt;
            refresh.UpdateAt = DateTime.Now;
            refresh.ExpiryDate = DateTime.Now.AddHours(6);
            refresh.TokenRefresh = tokenDto.TokenRefresh;
            _userContext.Entry(refresh).State = EntityState.Modified;
            _userContext.Entry(result).CurrentValues.SetValues(refresh);

            user.RefreshTokenId = refresh.Id;
            user.RefreshToken = result;
            _userContext.Entry(user).State = EntityState.Modified;

            await _userContext.SaveChangesAsync();

            return tokenDto;
        }

        public async Task<UserDto> InvalidadeUserRefreshToken(int userId)
        {
            var user = await _userContext.User.FirstOrDefaultAsync(x => x.UserID == userId && x.Active == true);

            RefreshToken token = user.RefreshToken;
            if (token == null)
                return null;
            token.ExpiryDate = DateTime.Now.AddHours(-6);
            token.Invalidated = true;
            _userContext.Entry(token).State = EntityState.Modified;
            await _userContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUserAsync(UserDto userDto)
        {
            var item = _mapper.Map<User>(userDto);
            item.CreateAt = DateTime.UtcNow;
            item.Active = true;
            _userContext.User.Add(item);
            await _userContext.SaveChangesAsync();
            userDto.UserID = item.UserID;
            return userDto;
        }

        public UserDto SelectByUserJwtId(string jwtId)
        {
            throw new NotImplementedException();
        }

        public List<PermissionDto> SelectPermissions(UserDto user)
        {
            return _userContext.Permission.Where(p => p.ProfileId == user.UserID).ToList();
        }

        public async Task<User> CreateUserAsync(UserLogin userLogin)
        {
            var item = _mapper.Map<User>(userLogin);
            item.CreateAt = DateTime.UtcNow;
            item.Active = true;
            _userContext.User.Add(item);
            await _userContext.SaveChangesAsync();
            return item;
        }

    }
}
