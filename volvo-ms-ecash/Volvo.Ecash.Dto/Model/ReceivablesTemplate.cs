using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class ReceivablesTemplate
    {
        public BankAccount MainAccount { get; set; }
        public List<Category> Categories { get; set; }
        public List<DomainModel> Domains { get; set; }
    }
}
