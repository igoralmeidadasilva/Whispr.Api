using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whispr.Domain.Features.Entities.RefreshTokens;

namespace Whispr.Infrastructure.Core.Data.Configurations;

internal sealed class RefreshTokenConfiguration : EntityConfiguration<RefreshToken>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        base.Configure(builder);

        builder.ToTable("refresh_tokens");

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.OwnsOne(x => x.TokenHash, hash =>
        {
            hash.Property(h => h.Value)
                .HasColumnName("token_hash")
                .HasColumnType("char(64)")
                .HasMaxLength(Domain.Constants.Constraints.RefreshToken.TokenLength)
                .IsRequired();
        });

        builder.Property(x => x.ExpirationAtUtc)
            .HasColumnName("expiration_at_utc")
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(x => x.RevokedAtUtc)
            .HasColumnName("revoked_at_utc");

        builder.Ignore(x => x.IsRevoked);
        builder.Ignore(x => x.IsExpired);
        builder.Ignore(x => x.IsActive);
    }
}