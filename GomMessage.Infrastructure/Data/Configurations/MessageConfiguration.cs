using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");
        builder.ConfigureEntityId();
        builder.ConfigureCreatedAtColumn();

        builder.Property(message => message.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(message => message.ConversationId)
            .HasColumnName("conversation_id")
            .IsRequired();

        builder.Property(message => message.AgentId)
            .HasColumnName("agent_id");

        builder.Property(message => message.ExternalMessageId)
            .HasColumnName("external_message_id")
            .HasMaxLength(500);

        builder.Property(message => message.SenderType)
            .HasColumnName("sender_type")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                senderType => EnumDatabaseValues.ToDatabaseValue(senderType),
                value => EnumDatabaseValues.ToSenderType(value));

        builder.Property(message => message.SenderName)
            .HasColumnName("sender_name")
            .HasMaxLength(255);

        builder.Property(message => message.SenderExternalId)
            .HasColumnName("sender_external_id")
            .HasMaxLength(255);

        builder.Property(message => message.Content)
            .HasColumnName("content")
            .HasColumnType("text");

        builder.Property(message => message.ContentType)
            .HasColumnName("content_type")
            .HasMaxLength(50)
            .HasDefaultValue(GomMessage.Domain.Entities.Enums.MessageContentType.Text)
            .HasConversion(
                contentType => contentType.HasValue ? EnumDatabaseValues.ToDatabaseValue(contentType.Value) : null,
                value => string.IsNullOrWhiteSpace(value) ? null : EnumDatabaseValues.ToMessageContentType(value));

        builder.Property(message => message.Attachments)
            .HasColumnName("attachments")
            .HasColumnType("jsonb")
            .HasNullableJsonContentConversion();

        builder.Property(message => message.SentAt)
            .HasColumnName("sent_at")
            .HasColumnType("timestamp with time zone");

        builder.Property(message => message.RawData)
            .HasColumnName("raw_data")
            .HasColumnType("jsonb")
            .HasNullableJsonContentConversion();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(message => message.TenantId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_messages_tenant");

        builder.HasOne<Conversation>()
            .WithMany()
            .HasForeignKey(message => message.ConversationId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_messages_conversation");

        builder.HasOne<Agent>()
            .WithMany()
            .HasForeignKey(message => message.AgentId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_messages_agent");

        builder.HasIndex(message => message.ConversationId)
            .HasDatabaseName("idx_messages_conversation_id");

        builder.HasIndex(message => message.TenantId)
            .HasDatabaseName("idx_messages_tenant_id");

        builder.HasIndex(message => message.AgentId)
            .HasDatabaseName("idx_messages_agent_id");

        builder.HasIndex(message => message.SentAt)
            .HasDatabaseName("idx_messages_sent_at");

        builder.HasIndex(message => new { message.ConversationId, message.ExternalMessageId })
            .IsUnique()
            .HasDatabaseName("uq_messages_conversation_external");
    }
}
