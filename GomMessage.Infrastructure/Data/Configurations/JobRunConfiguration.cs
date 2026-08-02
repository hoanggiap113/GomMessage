using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class JobRunConfiguration : IEntityTypeConfiguration<JobRun>
{
    public void Configure(EntityTypeBuilder<JobRun> builder)
    {
        builder.ToTable("job_runs");
        builder.ConfigureEntityId();
        builder.ConfigureCreatedAtColumn();

        builder.Property(jobRun => jobRun.JobId)
            .HasColumnName("job_id")
            .IsRequired();

        builder.Property(jobRun => jobRun.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(jobRun => jobRun.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                status => EnumDatabaseValues.ToDatabaseValue(status),
                value => EnumDatabaseValues.ToJobRunStatus(value));

        builder.Property(jobRun => jobRun.StartedAt)
            .HasColumnName("started_at")
            .HasColumnType("timestamp with time zone");

        builder.Property(jobRun => jobRun.FinishedAt)
            .HasColumnName("finished_at")
            .HasColumnType("timestamp with time zone");

        builder.Property(jobRun => jobRun.TotalConversations)
            .HasColumnName("total_conversations")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(jobRun => jobRun.EvaluatedConversations)
            .HasColumnName("evaluated_conversations")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(jobRun => jobRun.SkippedConversations)
            .HasColumnName("skipped_conversations")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(jobRun => jobRun.Summary)
            .HasColumnName("summary")
            .HasColumnType("jsonb")
            .HasNullableJsonContentConversion();

        builder.Property(jobRun => jobRun.ErrorMessage)
            .HasColumnName("error_message")
            .HasColumnType("text");

        builder.HasOne<Job>()
            .WithMany()
            .HasForeignKey(jobRun => jobRun.JobId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_job_runs_job");

        builder.HasIndex(jobRun => jobRun.JobId)
            .HasDatabaseName("idx_job_runs_job_id");

        builder.HasIndex(jobRun => jobRun.TenantId)
            .HasDatabaseName("idx_job_runs_tenant_id");

        builder.HasIndex(jobRun => jobRun.Status)
            .HasDatabaseName("idx_job_runs_status");
    }
}
