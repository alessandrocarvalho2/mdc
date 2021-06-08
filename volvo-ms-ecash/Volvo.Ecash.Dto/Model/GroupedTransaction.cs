using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class GroupedTransaction
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public List<Transaction> Transactions { get; set; }


        public int? BankAccountId { get; set; }

        public int? OperationId { get; set; }

        public string InOut { get; set; }

        public DateTime? Date { get; set; }

        public int? DocumentUploadId { get; set; }

        public int? CategoryId { get; set; }

        public int? DomainId { get; set; }

        public Int64? ConciliationId { get; set; }

        public int? RowNumber { get; set; }
    }
}
