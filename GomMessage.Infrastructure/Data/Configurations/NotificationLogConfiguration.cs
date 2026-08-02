using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class NotificationLogConfiguration : IEntityTypeConfiguration<NotificationLog>
{
    public void Configure(EntityTypeBuilder<NotificationLog> builder)
    {
        builder.ToTable("notification_logs");
        builder.ConfigureEntityId();
        builder.ConfigureCreatedAtColumn();

        builder.Property(notificationLog => notificationLog.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(notificationLog => notificationLog.JobId)
            .HasColumnName("job_id");

        builder.Property(notificationLog => notificationLog.JobRunId)
            .HasColumnName("job_run_id");

        builder.Property(notificationLog => notificationLog.Channel)
            .HasColumnName("channel")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                channel => EnumDatabaseValues.ToDatabaseValue(channel),
                value => EnumDatabaseValues.ToNotificationChannel(value));

        builder.Property(notificationLog => notificationLog.Recipient)
            .HasColumnName("recipient")
            .HasMaxLength(255);

        builder.Property(notificationLog => notificationLog.Subject)
            .HasColumnName("subject")
            .HasMaxLength(500);

        builder.Property(notificationLog => notificationLog.Body)
            .HasColumnName("body")
            .HasColumnType("text");

        builder.Property(notificationLog => notificationLog.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                status => EnumDatabaseValues.ToDatabaseValue(status),
                value => EnumDatabaseValues.ToNotificationStatus(value));

        builder.Property(notificationLog => notificationLog.ErrorMessage)
            .HasColumnName("error_message")
            .HasColumnType("text");

        builder.Property(notificationLog => notificationLog.SentAt)
            .HasColumnName("sent_at")
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(notificationLog => notificationLog.TenantId)
            .HasDatabaseName("idx_notification_logs_tenant_id");

        builder.HasIndex(notificationLog => notificationLog.JobRunId)
            .HasDatabaseName("idx_notification_logs_job_run_id");
    }
}
