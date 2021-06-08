using Volvo.Ecash.Dto.Model;
using System.Threading.Tasks;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface IEmailService
    {

        Task SendEmailAsync(AuthenticationResultDto authenticationResultDto, ResetPasswordDto resetPasswordDto, string newPassword,string token);
        
    }
}
