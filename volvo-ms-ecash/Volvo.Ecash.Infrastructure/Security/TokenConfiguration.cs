namespace Volvo.Ecash.Application.Utils.Security
{
    public class TokenConfiguration
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        //public int Seconds { get; set; }
        public string SecurityKey { get; set; }
    }
}
