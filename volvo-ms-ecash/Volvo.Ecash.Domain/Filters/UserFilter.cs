using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Domain.Filters
{
    public class UserFilter : PaginationFilter
    {
        public long? Id { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Search { get; set; }
        public DateTime? LastAcess { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
