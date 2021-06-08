namespace Volvo.Ecash.Domain.Entities
{
    public class MasterPassword : BaseEntity
    {
        public MasterPassword()
        { }

        public MasterPassword(string _password)
        {
            Password = _password;
        }

        public string Password { get; set; }

    }

}
