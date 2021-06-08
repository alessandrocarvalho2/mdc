using System;

namespace Volvo.Ecash.Dto.Model
{
    public class AuditLogDto
    {
        public int PersonaId { get; set; }
        public string PersonaName { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public DateTime EventDate { get; set; }
        public string TypeLog { get; set; }
        public string MicroService { get; set; }
    }
}
