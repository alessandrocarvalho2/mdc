using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Volvo.Ecash.Application.Utils.Security
{
    public class SigninConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }
        public string Audience { get;}
        public string Issuer { get;}
        public int Seconds { get; }

        public SigninConfigurations(IConfiguration _configuration)
        {
            Key = new SymmetricSecurityKey(
                          Encoding.UTF8.GetBytes(_configuration["TokenConfigurations:SecurityKey"]));
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            Audience = _configuration["TokenConfigurations:Audience"];
            Issuer = _configuration["TokenConfigurations:Issuer"];
            Seconds = int.Parse(_configuration["TokenConfigurations:Seconds"]);
        }
    }
}
