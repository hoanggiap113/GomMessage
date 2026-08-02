using GomMessage.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> ConfigureEntityId<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Entity
    {
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();
        builder.Ignore(entity => entity.DomainEvents);
        return builder;
    }

    public static EntityTypeBuilder<TEntity> ConfigureAuditableColumns<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : AuditableEntity
    {
        builder.Property(entity => entity.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(entity => entity.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        return builder;
    }

    public static EntityTypeBuilder<TEntity> ConfigureCreatedAtColumn<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Entity
    {
        builder.Property<DateTimeOffset>("CreatedAt")
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        return builder;
    }
}
