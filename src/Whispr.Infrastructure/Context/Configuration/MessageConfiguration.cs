using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whispr.Domain;
using Whispr.Domain.Entities;

namespace Whispr.Infrastructure.Context.Configuration;

internal sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");

        builder.HasKey(message => message.Id);

        builder.HasOne(message => message.Sender)
            .WithMany(user => user.Messages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(message => message.Content)
           .HasColumnName("content")
           .HasMaxLength(Constants.Constraints.Message.ContentMaxLength)
           .IsRequired();

        builder.Property(message => message.CreatedAtUtc)
           .HasColumnName("created_at_utc")
           .IsRequired();
    }
}
