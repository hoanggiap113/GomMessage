using GomMessage.Domain.Common;
using GomMessage.Domain.Entities.Enums;
using GomMessage.Domain.Events;
using GomMessage.Domain.Exceptions;
using GomMessage.Domain.ValueObjects;

namespace GomMessage.Domain.Entities;

public sealed class JobRun : AggregateRoot
{
    public Guid JobId { get; private set; }
    public Guid TenantId { get; private set; }
    public JobRunStatus Status { get; private set; }
    public DateTimeOffset? StartedAt { get; private set; }
    public DateTimeOffset? FinishedAt { get; private set; }
    public int TotalConversations { get; private set; }
    public int EvaluatedConversations { get; private set; }
    public int SkippedConversations { get; private set; }
    public JsonContent? Summary { get; private set; }
    public string? ErrorMessage { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private JobRun()
    {
    }

    private JobRun(Guid id, Guid jobId, Guid tenantId) : base(id)
    {
        JobId = Guard.AgainstEmptyGuid(jobId, nameof(jobId));
        TenantId = Guard.AgainstEmptyGuid(tenantId, nameof(tenantId));
        Status = JobRunStatus.Pending;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static JobRun Create(Guid jobId, Guid tenantId)
    {
        return new JobRun(Guid.NewGuid(), jobId, tenantId);
    }

    public void Start(DateTimeOffset? startedAt = null)
    {
        if (Status != JobRunStatus.Pending)
        {
            throw new DomainException("Only pending job run can be started.");
        }

        Status = JobRunStatus.Running;
        StartedAt = startedAt ?? DateTimeOffset.UtcNow;
        AddDomainEvent(new JobRunStartedDomainEvent(Id, JobId, TenantId));
    }

    public void UpdateProgress(int totalConversations, int evaluatedConversations, int skippedConversations)
    {
        if (totalConversations < 0 || evaluatedConversations < 0 || skippedConversations < 0)
        {
            throw new DomainException("Job run counters must not be negative.");
        }

        TotalConversations = totalConversations;
        EvaluatedConversations = evaluatedConversations;
        SkippedConversations = skippedConversations;
    }

    public void Complete(JobRunStatus status, string? summaryJson = null, string? errorMessage = null, DateTimeOffset? finishedAt = null)
    {
        if (status is not (JobRunStatus.Success or JobRunStatus.Failed or JobRunStatus.Partial))
        {
            throw new DomainException("Completed job run status must be Success, Failed or Partial.");
        }

        Status = status;
        Summary = JsonContent.CreateOrNull(summaryJson);
        ErrorMessage = string.IsNullOrWhiteSpace(errorMessage) ? null : errorMessage;
        FinishedAt = finishedAt ?? DateTimeOffset.UtcNow;
        AddDomainEvent(new JobRunCompletedDomainEvent(Id, JobId, TenantId, Status.ToString()));
    }
}
