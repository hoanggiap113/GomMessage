using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("invitations");
        builder.ConfigureEntityId();
        builder.ConfigureCreatedAtColumn();

        builder.Property(invitation => invitation.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(invitation => invitation.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired()
            .HasEmailAddressConversion();

        builder.Property(invitation => invitation.Role)
            .HasColumnName("role")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                role => EnumDatabaseValues.ToDatabaseValue(role),
                value => EnumDatabaseValues.ToTenantRole(value));

        builder.Property(invitation => invitation.Token)
            .HasColumnName("token")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(invitation => invitation.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                status => EnumDatabaseValues.ToDatabaseValue(status),
                value => EnumDatabaseValues.ToInvitationStatus(value));

        builder.Property(invitation => invitation.InvitedById)
            .HasColumnName("invited_by")
            .IsRequired();

        builder.Property(invitation => invitation.ExpiresAt)
            .HasColumnName("expires_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(invitation => invitation.TenantId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_invitations_tenant");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(invitation => invitation.InvitedById)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_invitations_invited_by");

        builder.HasIndex(invitation => invitation.Email)
            .HasDatabaseName("idx_invitations_email");

        builder.HasIndex(invitation => invitation.Token)
            .IsUnique()
            .HasDatabaseName("idx_invitations_token");

        builder.HasIndex(invitation => invitation.TenantId)
            .HasDatabaseName("idx_invitations_tenant_id");
    }
}
