using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Configurations;

internal sealed class JobResultConfiguration : IEntityTypeConfiguration<JobResult>
{
    public void Configure(EntityTypeBuilder<JobResult> builder)
    {
        builder.ToTable("job_results");
        builder.ConfigureEntityId();
        builder.ConfigureCreatedAtColumn();

        builder.Property(jobResult => jobResult.JobRunId)
            .HasColumnName("job_run_id")
            .IsRequired();

        builder.Property(jobResult => jobResult.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(jobResult => jobResult.ConversationId)
            .HasColumnName("conversation_id")
            .IsRequired();

        builder.Property(jobResult => jobResult.AgentId)
            .HasColumnName("agent_id");

        builder.Property(jobResult => jobResult.OverallScore)
            .HasColumnName("overall_score")
            .HasNullableScoreConversion();

        builder.Property(jobResult => jobResult.Passed)
            .HasColumnName("passed");

        builder.Property(jobResult => jobResult.Severity)
            .HasColumnName("severity")
            .HasMaxLength(50)
            .HasConversion(
                severity => severity.HasValue ? EnumDatabaseValues.ToDatabaseValue(severity.Value) : null,
                value => string.IsNullOrWhiteSpace(value) ? null : EnumDatabaseValues.ToSeverityLevel(value));

        builder.Property(jobResult => jobResult.Summary)
            .HasColumnName("summary")
            .HasColumnType("text");

        builder.Property(jobResult => jobResult.Detail)
            .HasColumnName("detail")
            .HasColumnType("jsonb")
            .HasNullableJsonContentConversion();

        builder.Property(jobResult => jobResult.AiRawResponse)
            .HasColumnName("ai_raw_response")
            .HasColumnType("text");

        builder.Property(jobResult => jobResult.Confidence)
            .HasColumnName("confidence")
            .HasNullableConfidenceScoreConversion();

        builder.Property(jobResult => jobResult.NotifiedAt)
            .HasColumnName("notified_at")
            .HasColumnType("timestamp with time zone");

        builder.HasOne<JobRun>()
            .WithMany()
            .HasForeignKey(jobResult => jobResult.JobRunId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_job_results_job_run");

        builder.HasOne<Conversation>()
            .WithMany()
            .HasForeignKey(jobResult => jobResult.ConversationId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_job_results_conversation");

        builder.HasOne<Agent>()
            .WithMany()
            .HasForeignKey(jobResult => jobResult.AgentId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_job_results_agent");

        builder.HasIndex(jobResult => jobResult.JobRunId)
            .HasDatabaseName("idx_job_results_job_run_id");

        builder.HasIndex(jobResult => jobResult.ConversationId)
            .HasDatabaseName("idx_job_results_conversation_id");

        builder.HasIndex(jobResult => jobResult.AgentId)
            .HasDatabaseName("idx_job_results_agent_id");

        builder.HasIndex(jobResult => jobResult.Severity)
            .HasDatabaseName("idx_job_results_severity");

        builder.HasIndex(jobResult => new { jobResult.JobRunId, jobResult.ConversationId })
            .IsUnique()
            .HasDatabaseName("uq_job_results_run_conv");
    }
}
