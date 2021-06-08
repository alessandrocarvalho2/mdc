using System;

namespace Volvo.Ecash.Dto.Model
{
    public class AccessLogDto
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public int AcessLogTypeId { get; set; }
        public string AcessLogType { get; set; }
        public DateTime EventDate { get; set; }
        public string TypeLog { get; set; }        
        public string MicroService { get; set; }
    }
}
