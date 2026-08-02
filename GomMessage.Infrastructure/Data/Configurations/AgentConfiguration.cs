using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class AgentConfiguration : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.ToTable("agents");
        builder.ConfigureEntityId();
        builder.ConfigureAuditableColumns();

        builder.Property(agent => agent.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(agent => agent.ExternalId)
            .HasColumnName("external_id")
            .HasMaxLength(255);

        builder.Property(agent => agent.Name)
            .HasColumnName("name")
            .HasMaxLength(255);

        builder.Property(agent => agent.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .HasNullableEmailAddressConversion();

        builder.Property(agent => agent.AvatarUrl)
            .HasColumnName("avatar_url")
            .HasMaxLength(500);

        builder.Property(agent => agent.Metadata)
            .HasColumnName("metadata")
            .HasColumnType("jsonb")
            .HasNullableJsonContentConversion();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(agent => agent.TenantId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_agents_tenant");

        builder.HasIndex(agent => agent.TenantId)
            .HasDatabaseName("idx_agents_tenant_id");

        builder.HasIndex(agent => new { agent.TenantId, agent.ExternalId })
            .IsUnique()
            .HasDatabaseName("uq_agents_tenant_external");
    }
}
