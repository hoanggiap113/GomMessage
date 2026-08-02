using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("conversations");
        builder.ConfigureEntityId();
        builder.ConfigureAuditableColumns();

        builder.Property(conversation => conversation.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(conversation => conversation.ChannelId)
            .HasColumnName("channel_id")
            .IsRequired();

        builder.Property(conversation => conversation.ExternalConversationId)
            .HasColumnName("external_conversation_id")
            .HasMaxLength(255);

        builder.Property(conversation => conversation.ExternalUserId)
            .HasColumnName("external_user_id")
            .HasMaxLength(255);

        builder.Property(conversation => conversation.CustomerName)
            .HasColumnName("customer_name")
            .HasMaxLength(255);

        builder.Property(conversation => conversation.LastMessageAt)
            .HasColumnName("last_message_at")
            .HasColumnType("timestamp with time zone");

        builder.Property(conversation => conversation.MessageCount)
            .HasColumnName("message_count")
            .HasDefaultValue(0);

        builder.Property(conversation => conversation.Metadata)
            .HasColumnName("metadata")
            .HasColumnType("jsonb")
            .HasNullableJsonContentConversion();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(conversation => conversation.TenantId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_conversations_tenant");

        builder.HasOne<Channel>()
            .WithMany()
            .HasForeignKey(conversation => conversation.ChannelId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_conversations_channel");

        builder.HasIndex(conversation => conversation.TenantId)
            .HasDatabaseName("idx_conversations_tenant_id");

        builder.HasIndex(conversation => conversation.ChannelId)
            .HasDatabaseName("idx_conversations_channel_id");

        builder.HasIndex(conversation => conversation.LastMessageAt)
            .HasDatabaseName("idx_conversations_last_message_at");

        builder.HasIndex(conversation => new { conversation.ChannelId, conversation.ExternalConversationId })
            .IsUnique()
            .HasDatabaseName("uq_conversations_channel_external");
    }
}
