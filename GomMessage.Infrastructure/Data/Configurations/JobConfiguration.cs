using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("jobs");
        builder.ConfigureEntityId();
        builder.ConfigureAuditableColumns();

        builder.Property(job => job.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(job => job.JobType)
            .HasColumnName("job_type")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                jobType => EnumDatabaseValues.ToDatabaseValue(jobType),
                value => EnumDatabaseValues.ToJobType(value));

        builder.Property(job => job.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(job => job.Description)
            .HasColumnName("description")
            .HasColumnType("text");

        builder.Property(job => job.InputChannelIds)
            .HasColumnName("input_channel_ids")
            .HasColumnType("jsonb")
            .HasNullableJsonContentConversion();

        builder.Property(job => job.RulesContent)
            .HasColumnName("rules_content")
            .HasColumnType("text");

        builder.Property(job => job.RulesConfig)
            .HasColumnName("rules_config")
            .HasColumnType("jsonb")
            .HasNullableJsonContentConversion();

        builder.Property(job => job.SkipConditions)
            .HasColumnName("skip_conditions")
            .HasColumnType("jsonb")
            .HasNullableJsonContentConversion();

        builder.Property(job => job.AiProvider)
            .HasColumnName("ai_provider")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(job => job.AiModel)
            .HasColumnName("ai_model")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(job => job.Outputs)
            .HasColumnName("outputs")
            .HasColumnType("jsonb")
            .HasNullableJsonContentConversion();

        builder.Property(job => job.ScheduleType)
            .HasColumnName("schedule_type")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                scheduleType => EnumDatabaseValues.ToDatabaseValue(scheduleType),
                value => EnumDatabaseValues.ToJobScheduleType(value));

        builder.Property(job => job.ScheduleCron)
            .HasColumnName("schedule_cron")
            .HasMaxLength(100)
            .HasNullableCronExpressionConversion();

        builder.Property(job => job.Active)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(job => job.LastRunAt)
            .HasColumnName("last_run_at")
            .HasColumnType("timestamp with time zone");

        builder.Property(job => job.LastRunStatus)
            .HasColumnName("last_run_status")
            .HasMaxLength(20);

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(job => job.TenantId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_jobs_tenant");

        builder.HasIndex(job => job.TenantId)
            .HasDatabaseName("idx_jobs_tenant_id");

        builder.HasIndex(job => job.Active)
            .HasDatabaseName("idx_jobs_is_active");
    }
}
