using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.ToTable("channels");
        builder.ConfigureEntityId();
        builder.ConfigureAuditableColumns();

        builder.Property(channel => channel.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(channel => channel.ChannelType)
            .HasColumnName("channel_type")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                channelType => EnumDatabaseValues.ToDatabaseValue(channelType),
                value => EnumDatabaseValues.ToChannelType(value));

        builder.Property(channel => channel.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(channel => channel.ExternalId)
            .HasColumnName("external_id")
            .HasMaxLength(255);

        builder.Property(channel => channel.CredentialsEncrypted)
            .HasColumnName("credentials_encrypted")
            .HasColumnType("text");

        builder.Property(channel => channel.Active)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(channel => channel.LastSyncAt)
            .HasColumnName("last_sync_at")
            .HasColumnType("timestamp with time zone");

        builder.Property(channel => channel.LastSyncStatus)
            .HasColumnName("last_sync_status")
            .HasMaxLength(50)
            .HasConversion(
                status => status.HasValue ? EnumDatabaseValues.ToDatabaseValue(status.Value) : null,
                value => string.IsNullOrWhiteSpace(value) ? null : EnumDatabaseValues.ToSyncStatus(value));

        builder.Property(channel => channel.LastSyncError)
            .HasColumnName("last_sync_error")
            .HasColumnType("text");

        builder.Property(channel => channel.Metadata)
            .HasColumnName("metadata")
            .HasColumnType("jsonb")
            .HasNullableJsonContentConversion();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(channel => channel.TenantId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_channels_tenant");

        builder.HasIndex(channel => channel.TenantId)
            .HasDatabaseName("idx_channels_tenant_id");

        builder.HasIndex(channel => channel.ChannelType)
            .HasDatabaseName("idx_channels_type");

        builder.HasIndex(channel => new { channel.TenantId, channel.ChannelType, channel.ExternalId })
            .IsUnique()
            .HasDatabaseName("uq_channels_tenant_external");
    }
}
