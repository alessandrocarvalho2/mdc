using System.Collections.Generic;


namespace Volvo.Ecash.Dto.Model
{
    public class LoginCreationDto
    {
        public LoginCreationDto()
        {
            Messages = new List<string>();
            ChildLogin = new List<string>();
        }

        public int LoginId { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ProfileCode { get; set; }
        public bool Active { get; set; }
        public bool CreatedSuccessfully { get; set; }        
        public List<string> ChildLogin { get; set; }
        public List<string> Messages { get; set; }
    }
}
