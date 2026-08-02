using GomMessage.Domain.Common;
using GomMessage.Domain.Entities.Enums;
using GomMessage.Domain.Events;
using GomMessage.Domain.ValueObjects;

namespace GomMessage.Domain.Entities;

public sealed class JobResult : AggregateRoot
{
    public Guid JobRunId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid ConversationId { get; private set; }
    public Guid? AgentId { get; private set; }
    public Score? OverallScore { get; private set; }
    public bool? Passed { get; private set; }
    public SeverityLevel? Severity { get; private set; }
    public string? Summary { get; private set; }
    public JsonContent? Detail { get; private set; }
    public string? AiRawResponse { get; private set; }
    public ConfidenceScore? Confidence { get; private set; }
    public DateTimeOffset? NotifiedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private JobResult()
    {
    }

    private JobResult(
        Guid id,
        Guid jobRunId,
        Guid tenantId,
        Guid conversationId,
        Guid? agentId,
        Score? overallScore,
        bool? passed,
        SeverityLevel? severity,
        string? summary,
        JsonContent? detail,
        string? aiRawResponse,
        ConfidenceScore? confidence) : base(id)
    {
        JobRunId = Guard.AgainstEmptyGuid(jobRunId, nameof(jobRunId));
        TenantId = Guard.AgainstEmptyGuid(tenantId, nameof(tenantId));
        ConversationId = Guard.AgainstEmptyGuid(conversationId, nameof(conversationId));
        AgentId = agentId == Guid.Empty ? null : agentId;
        OverallScore = overallScore;
        Passed = passed;
        Severity = severity;
        Summary = string.IsNullOrWhiteSpace(summary) ? null : summary;
        Detail = detail;
        AiRawResponse = string.IsNullOrWhiteSpace(aiRawResponse) ? null : aiRawResponse;
        Confidence = confidence;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static JobResult Create(
        Guid jobRunId,
        Guid tenantId,
        Guid conversationId,
        Guid? agentId = null,
        double? overallScore = null,
        bool? passed = null,
        SeverityLevel? severity = null,
        string? summary = null,
        string? detailJson = null,
        string? aiRawResponse = null,
        double? confidence = null)
    {
        var result = new JobResult(
            Guid.NewGuid(),
            jobRunId,
            tenantId,
            conversationId,
            agentId,
            overallScore.HasValue ? Score.Create(overallScore.Value) : null,
            passed,
            severity,
            summary,
            JsonContent.CreateOrNull(detailJson),
            aiRawResponse,
            confidence.HasValue ? ConfidenceScore.Create(confidence.Value) : null);

        result.AddDomainEvent(new JobResultCreatedDomainEvent(result.Id, result.JobRunId, result.ConversationId, result.AgentId, result.Severity?.ToString()));
        return result;
    }

    public void MarkNotified(DateTimeOffset? notifiedAt = null)
    {
        NotifiedAt = notifiedAt ?? DateTimeOffset.UtcNow;
    }
}
