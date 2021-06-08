namespace Volvo.Ecash.Domain.Entities
{
    public class LoginUser : BaseEntity
    {
        public LoginUser()
        { }

        public LoginUser(string _login, string _password)
        {
            Login = _login;
            Password = _password;
        }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool? Active { get; set; }       
        public int? RefreshTokenId { get; set; }
        public RefreshToken RefreshToken { get; set; }

    }

}
