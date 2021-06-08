namespace Volvo.Ecash.Domain.Entities
{
    public class TokenJwt : BaseEntity
    {
        public string AccessToken { get; set; }
        public bool Autenticated { get; set; }
        public string Expiration { get; set; }        
        public string Message { get; set; }

    }
}
