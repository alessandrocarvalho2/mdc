using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volvo.Ecash.Domain.Entities;

namespace Volvo.Ecash.Infrastructure.Configuration
{
    public class RefreshConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_token");

            builder.Property(p => p.Id)
             .HasColumnName("id")
             .ValueGeneratedOnAdd();

            builder.Property(p => p.CreateAt)
            .HasColumnName("create_at");

            builder.Property(p => p.UpdateAt)
            .HasColumnName("update_at");
                        
            builder.Property(p => p.TokenRefresh)
                .HasColumnName("token_refresh");

            builder.Property(p => p.TokenJwt)
            .HasColumnName("token_jwt");

            builder.Property(p => p.JwtId)
            .HasColumnName("jwt_id");

            builder.Property(p => p.ExpiryDate)
            .HasColumnName("expiry_date");

            builder.Property(p => p.Invalidated)
            .HasColumnName("invalidated");
        }
    }
}
