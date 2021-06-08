using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volvo.Ecash.Domain.Entities;

namespace Volvo.Ecash.Dto.Model
{

    [Table("user")]
    public class UserDto
    {

        [Column("user_id")]
        public int UserID { get; set; }
        [Column("login")]
        public string Login { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("update_at")]
        public DateTime UpdateAt { get; set; }
        [Column("create_at")]
        public DateTime CreateAt { get; set; }
        [Column("active")]
        public bool Active { get; set; }
        [Column("refresh_token_id")]
        public int RefreshTokenId { get; set; }

        public string StatusLogin { get; set; }
    }
}
