using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class UserTenantConfiguration : IEntityTypeConfiguration<UserTenant>
{
    public void Configure(EntityTypeBuilder<UserTenant> builder)
    {
        builder.ToTable("user_tenants");
        builder.ConfigureEntityId();
        builder.ConfigureCreatedAtColumn();

        builder.Property(membership => membership.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(membership => membership.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(membership => membership.Role)
            .HasColumnName("role")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                role => EnumDatabaseValues.ToDatabaseValue(role),
                value => EnumDatabaseValues.ToTenantRole(value));

        builder.Property(membership => membership.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                status => EnumDatabaseValues.ToDatabaseValue(status),
                value => EnumDatabaseValues.ToMembershipStatus(value));

        builder.Property(membership => membership.InvitedById)
            .HasColumnName("invited_by");

        builder.Property(membership => membership.JoinedAt)
            .HasColumnName("joined_at")
            .HasColumnType("timestamp with time zone");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(membership => membership.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_user_tenants_user");

        builder.HasOne<Tenant>()
            .WithMany(t => t.UserTenants)
            .HasForeignKey(membership => membership.TenantId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_user_tenants_tenant");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(membership => membership.InvitedById)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_user_tenants_invited_by");

        builder.HasIndex(membership => new { membership.UserId, membership.TenantId })
            .IsUnique()
            .HasDatabaseName("uq_user_tenant");

        builder.HasIndex(membership => membership.UserId)
            .HasDatabaseName("idx_user_tenants_user_id");

        builder.HasIndex(membership => membership.TenantId)
            .HasDatabaseName("idx_user_tenants_tenant_id");

        builder.HasIndex(membership => membership.Role)
            .HasDatabaseName("idx_user_tenants_role");
    }
}
