using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whispr.Domain.Features.Entities.PasswordResetTokens;

namespace Whispr.Infrastructure.Core.Data.Configurations;

internal sealed class PasswordResetTokenConfiguration : EntityConfiguration<PasswordResetToken>
{
    public override void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        base.Configure(builder);

        builder.ToTable("password_reset_tokens");

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.OwnsOne(token => token.TokenHash, hash =>
        {
            hash.Property(h => h.Value)
                .HasColumnName("token_hash")
                .HasColumnType("char(64)")
                .HasMaxLength(Domain.Constants.Constraints.PasswordResetToken.TokenLength)
                .IsRequired();
        });

        builder.Property(x => x.ExpirationAtUtc)
            .HasColumnName("expiration_at_utc")
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(x => x.UsedAtUtc)
            .HasColumnName("used_at_utc");

        builder.Ignore(x => x.IsExpired);
    }
}