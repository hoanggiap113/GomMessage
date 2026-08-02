using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.ConfigureEntityId();
        builder.ConfigureAuditableColumns();

        builder.Property(user => user.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired()
            .HasEmailAddressConversion();

        builder.Property(user => user.PasswordHash)
            .HasColumnName("password_hash")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(user => user.Name)
            .HasColumnName("name")
            .HasMaxLength(255);
        builder.Property(user => user.Telephone)
            .HasColumnName("telephone")
            .HasMaxLength(20);

        builder.Property(user => user.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                status => EnumDatabaseValues.ToDatabaseValue(status),
                value => EnumDatabaseValues.ToUserStatus(value));

        builder.Property(user => user.TokenVersion)
            .HasColumnName("token_version")
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasIndex(user => user.Email)
            .IsUnique()
            .HasDatabaseName("idx_users_email");

        builder.HasIndex(user => user.Status)
            .HasDatabaseName("idx_users_status");
    }
}
