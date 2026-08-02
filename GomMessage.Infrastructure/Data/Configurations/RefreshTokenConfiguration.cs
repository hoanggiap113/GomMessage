using GomMessage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");
        builder.ConfigureEntityId();
        builder.ConfigureCreatedAtColumn();

        builder.Property(refreshToken => refreshToken.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(refreshToken => refreshToken.TokenHash)
            .HasColumnName("token_hash")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(refreshToken => refreshToken.ExpiresAt)
            .HasColumnName("expires_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(refreshToken => refreshToken.Revoked)
            .HasColumnName("revoked")
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_refresh_tokens_user");

        builder.HasIndex(refreshToken => refreshToken.UserId)
            .HasDatabaseName("idx_refresh_tokens_user_id");

        builder.HasIndex(refreshToken => refreshToken.ExpiresAt)
            .HasDatabaseName("idx_refresh_tokens_expires_at");

        builder.HasIndex(refreshToken => refreshToken.Revoked)
            .HasDatabaseName("idx_refresh_tokens_revoked");

        builder.HasIndex(refreshToken => refreshToken.TokenHash)
            .IsUnique()
            .HasDatabaseName("idx_refresh_tokens_token_hash");
    }
}
