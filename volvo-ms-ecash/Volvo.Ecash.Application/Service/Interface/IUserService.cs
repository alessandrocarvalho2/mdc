using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface IUserService
    {
        Task<AuthenticationResultDto> GetByUser<V>(UserDto userDto) where V : AbstractValidator<UserDto>;
        Task<AuthenticationResultDto> GetByUser<V>(UserLogin userLogin) where V : AbstractValidator<UserLogin>;
        Task<AuthenticationResultDto> RefreshToken(RefreshTokenDto refreshTokenDto,bool getRefreshToken);
        Task<AuthenticationResultDto> UpdatePassword<V>(UserLogin userLogin) where V : AbstractValidator<UserLogin>;
        Task<AuthenticationResultDto> InvalidateRefreshToken(UserDto userDto);
        Task<object> CreateUser<V>(UserLogin userLogin) where V : AbstractValidator<UserLogin>;
        Task<object> UpdateUser(int idUser, UserDto user);
        Task<object> DeleteUser(int idUser);
        Task<object> DeleteMultipleUsers(List<int> userIds);
        Task<UserDto> SelectUserAsync(string user);
        Task<IEnumerable<User>> SelectUsersAsync();
        List<PermissionDto> SelectPermissions(UserDto user);
    }
}
