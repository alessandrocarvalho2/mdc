using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Context
{
    public class UtilsContext : DbContext
    {
        public DbSet<Holiday> Holidays { get; set; }

        public UtilsContext() : base() { }
        public UtilsContext(DbContextOptions<UtilsContext> options) : base(options) { }

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
