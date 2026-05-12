using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whispr.Domain.Features.Entities.User;

namespace Whispr.Infrastructure.Core.Data.Configurations;

internal sealed class UserConfiguration : EntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("users");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(Domain.Constants.Constraints.User.NameMaxLength);

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(Domain.Constants.Constraints.User.EmailMaxLength);

        builder.OwnsOne(user => user.PasswordHash, passwordBuilder =>
        {
            passwordBuilder.WithOwner();

            passwordBuilder.Property(password => password.Hash)
                .HasColumnName("password_hash")
                .IsRequired();
        });

        builder.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.DeletedAtUtc)
            .HasColumnName("deleted_at_utc");

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}