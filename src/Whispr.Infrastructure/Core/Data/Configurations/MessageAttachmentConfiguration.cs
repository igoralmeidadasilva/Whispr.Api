using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whispr.Domain.Features.Entities.MessageAttachments;

namespace Whispr.Infrastructure.Core.Data.Configurations;

internal sealed class MessageAttachmentConfiguration : EntityConfiguration<MessageAttachment>
{
    public override void Configure(EntityTypeBuilder<MessageAttachment> builder)
    {
        base.Configure(builder);

        builder.ToTable("message_attachments");

        builder.Property(x => x.MessageId)
            .HasColumnName("message_id")
            .IsRequired();

        builder.HasOne(x => x.Message)
            .WithMany()
            .HasForeignKey(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(x => x.StorageKey)
            .HasColumnName("storage_key")
            .HasMaxLength(Domain.Constants.Constraints.MessageAttachment.StorageKeyMaxLength)
            .IsRequired();

        builder.Property(x => x.FileName) 
            .HasColumnName("file_name")
            .HasMaxLength(Domain.Constants.Constraints.MessageAttachment.FileNameMaxLength)
            .IsRequired();

        builder.Property(x => x.ContentType)
            .HasColumnName("content_type")
            .HasMaxLength(Domain.Constants.Constraints.MessageAttachment.ContentTypeMaxLength)
            .IsRequired();

        builder.Property(x => x.SizeBytes)
            .HasColumnName("size_bytes")
            .HasMaxLength(Domain.Constants.Constraints.MessageAttachment.SizeBytesMaxLength)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.HasIndex(x => x.CreatedAtUtc);
    }
}