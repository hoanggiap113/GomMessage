namespace GomMessage.Domain.Events;

public sealed record JobResultCreatedDomainEvent(Guid JobResultId, Guid JobRunId, Guid ConversationId, Guid? AgentId, string? Severity) : DomainEvent;
