using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Domain.Entities
{
    [Table("refresh_token")]
    public class RefreshToken
    {
        public User User { get; set; }

        public int Id { get; set; }
        public string TokenRefresh { get; set; }

        public string TokenJwt { get; set; }

        public string JwtId { get; set; }

        public DateTime ExpiryDate { get; set; }        
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public bool Invalidated { get; set; }
                


    }
}
