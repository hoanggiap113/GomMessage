using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenants");
        builder.ConfigureEntityId();
        builder.ConfigureAuditableColumns();

        builder.Property(tenant => tenant.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(tenant => tenant.Slug)
            .HasColumnName("slug")
            .HasMaxLength(255)
            .IsRequired()
            .HasTenantSlugConversion();

        builder.Property(tenant => tenant.OwnerId)
            .HasColumnName("owner_id")
            .IsRequired();

        builder.Property(tenant => tenant.Settings)
            .HasColumnName("settings")
            .HasColumnType("jsonb")
            .HasNullableJsonContentConversion();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(tenant => tenant.OwnerId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_tenants_owner");

        builder.HasIndex(tenant => tenant.OwnerId)
            .HasDatabaseName("idx_tenants_owner_id");

        builder.HasIndex(tenant => tenant.Slug)
            .IsUnique()
            .HasDatabaseName("idx_tenants_slug");

        builder.HasIndex(tenant => tenant.Name)
            .IsUnique()
            .HasDatabaseName("uq_tenants_name");
    }
}
