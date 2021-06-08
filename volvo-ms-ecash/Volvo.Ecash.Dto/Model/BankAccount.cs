using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    [Table("bank_account")]
    public class BankAccount
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("account")]
        public string Account { get; set; }

        [Column("agency")]
        public string Agency { get; set; }

        [Column("nickname")]
        public string Nickname { get; set; }

        [Column("minimum_balance", TypeName = "numeric(30,2)")]
        public decimal MinimumBalance { get; set; }

        [Column("balance_tolerance", TypeName = "numeric(30,2)")]
        public decimal BalanceTolerance { get; set; }

        [Column("kpi_target", TypeName = "numeric(30,2)")]
        public decimal KpiTarget { get; set; }

        [Column("main_account")]
        public bool IsMainAccount { get; set; }

        [Column("bank_id")]
        public int BankId { get; set; }

        [Column("report_order")]
        public int ReportOrder { get; set; }

        [Column("kpi_order")]
        public int KpiOrder { get; set; }

        [Column("accounting_description")]
        public string AccountingDescription { get; set; }

        public Bank Bank { get; set; }

        public AccountBalance AccountBalance { get; set; }
    }
}
