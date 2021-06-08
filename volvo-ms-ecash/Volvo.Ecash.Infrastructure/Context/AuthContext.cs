using Microsoft.EntityFrameworkCore;
using Volvo.Ecash.Domain.Entities;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Configuration;
using Volvo.Ecash.Infrastructure.Seed;

namespace Volvo.Ecash.Infrastructure
{
    public class AuthContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RefreshConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Method intentionally left empty.
        }


    }


}
