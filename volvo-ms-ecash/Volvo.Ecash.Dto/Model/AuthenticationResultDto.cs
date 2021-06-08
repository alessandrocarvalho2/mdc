

namespace Volvo.Ecash.Dto.Model
{
    public class AuthenticationResultDto
    {


        public bool FirstAcess { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public bool Success { get; set; }

        public string Expiration { get; set; }

        public string Message { get; set; }



    }
}
