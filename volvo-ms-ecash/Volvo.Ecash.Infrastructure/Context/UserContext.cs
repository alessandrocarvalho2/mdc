using Microsoft.EntityFrameworkCore;
using Volvo.Ecash.Domain.Entities;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Seed;

namespace Volvo.Ecash.Infrastructure.Context
{
    public class UserContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<PermissionDto> Permission { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);

                entity.Property(e => e.UserID).HasColumnName("user_id");
                entity.Property(e => e.Login).HasColumnName("login");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.UpdateAt).HasColumnName("update_at");
                entity.Property(e => e.CreateAt).HasColumnName("create_at");
                entity.Property(e => e.Active).HasColumnName("active");
                entity.Property(e => e.RefreshTokenId).HasColumnName("refresh_token_id");

            });

            modelBuilder.Entity<PermissionDto>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Menu).HasColumnName("menu");
                entity.Property(e => e.TextInfo).HasColumnName("text_info");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Subtitle).HasColumnName("subtitle");
                entity.Property(e => e.ProfileId).HasColumnName("profile_id");

            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TokenRefresh).HasColumnName("token_refresh");
                entity.Property(e => e.TokenJwt).HasColumnName("token_jwt");
                entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
                entity.Property(e => e.Invalidated).HasColumnName("invalidated");
                entity.Property(e => e.JwtId).HasColumnName("jwt_id");
                entity.Property(e => e.CreateAt).HasColumnName("create_at");
                entity.Property(e => e.UpdateAt).HasColumnName("update_at");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Method intentionally left empty.
        }
    }
}
