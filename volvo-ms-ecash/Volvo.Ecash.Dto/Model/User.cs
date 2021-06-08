using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volvo.Ecash.Domain.Entities;

namespace Volvo.Ecash.Dto.Model
{
    [Table("user")]
    public class User
    {
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Active { get; set; }
        public int? RefreshTokenId { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
