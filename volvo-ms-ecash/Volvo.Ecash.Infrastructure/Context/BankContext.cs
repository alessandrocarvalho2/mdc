using Microsoft.EntityFrameworkCore;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Context
{
    public class BankContext : DbContext
    {
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<AccountBalance> AccountBalances { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<DocumentUploadDto> DocumentUploads { get; set; }
        public DbSet<DomainModel> Domains { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CashFlow> CashFlowTransactions { get; set; }
        public DbSet<LogTransactionClosed> LogsTransactionClosed { get; set; }
        public DbSet<CashFlowDetailed> CashFlowTransactionsDetaileds { get; set; }
        public DbSet<Conciliation> Conciliations { get; set; }

        public BankContext() : base() { }
        public BankContext(DbContextOptions<BankContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Method intentionally left empty.
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Method intentionally left empty.
        }


    }
}
