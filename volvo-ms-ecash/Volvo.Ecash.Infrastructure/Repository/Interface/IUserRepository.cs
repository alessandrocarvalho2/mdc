using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Ecash.Domain.Entities;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface IUserRepository
    {
        Task<RefreshTokenDto> InsertAsync(RefreshTokenDto item, int userId);
        Task<UserDto> SelectByUser(UserDto userDto);
        Task<UserDto> SelectByUser(UserLogin userLogin);
        Task<UserDto> SelectByUserUpdate(UserLogin userLogin);
        Task<RefreshTokenDto> UpdateAsync(RefreshTokenDto tokenDto, string tokenRefresh, int userId);
        RefreshTokenDto SelectRefreshToken(string token, string refreshToken);
        Task<RefreshToken> SelectRefreshTokenByUserId(int userId);
        Task<RefreshTokenDto> SelectRefreshTokenById(int id);
        UserDto SelectByUserJwtId(string jwtId);
        Task<UserDto> UpdatePasswordAsync(UserDto userDto);
        Task<UserDto> InvalidadeUserRefreshToken(int userId);
        Task<User> CreateUserAsync(UserLogin userLogin);
        Task<UserDto> SelectUser(string user);
        Task<bool> ValidMasterPassword(string password);
        Task<UserDto> UpdateUserAsync(int idUser, UserDto user);
        Task<bool> DeleteAsync(int idUser);        
        Task<UserDto> SelectUserById(int id);
        Task<RefreshTokenDto> SelectRefreshToken(string refreshToken);
        Task<UserDto> SelectUserMasterPassword(string user);
        Task<IEnumerable<User>> SelectUsersAsync();
        List<PermissionDto> SelectPermissions(UserDto user);
    }
}
