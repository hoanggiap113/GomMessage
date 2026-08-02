using GomMessage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class AppSettingsConfiguration : IEntityTypeConfiguration<AppSettings>
{
    public void Configure(EntityTypeBuilder<AppSettings> builder)
    {
        builder.ToTable("app_settings");
        builder.ConfigureEntityId();
        builder.ConfigureAuditableColumns();

        builder.Property(appSettings => appSettings.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(appSettings => appSettings.SettingKey)
            .HasColumnName("setting_key")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(appSettings => appSettings.ValueEncrypted)
            .HasColumnName("value_encrypted")
            .HasColumnType("text");

        builder.Property(appSettings => appSettings.ValuePlain)
            .HasColumnName("value_plain")
            .HasColumnType("text");

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(appSettings => appSettings.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(appSettings => appSettings.TenantId)
            .HasDatabaseName("idx_app_settings_tenant_id");

        builder.HasIndex(appSettings => new { appSettings.TenantId, appSettings.SettingKey })
            .IsUnique()
            .HasDatabaseName("uq_app_settings_tenant_key");
    }
}
