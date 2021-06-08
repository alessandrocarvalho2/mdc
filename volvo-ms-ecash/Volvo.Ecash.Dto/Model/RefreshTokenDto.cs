using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Volvo.Ecash.Dto.Model
{
    
    public class RefreshTokenDto
    {        
              
        public string TokenRefresh { get; set; }
        public string TokenJwt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Invalidated { get; set; }
        public string JwtId { get; set; }        
    }
}
