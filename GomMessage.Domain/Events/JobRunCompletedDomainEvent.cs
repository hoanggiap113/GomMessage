namespace GomMessage.Domain.Events;

public sealed record JobRunCompletedDomainEvent(Guid JobRunId, Guid JobId, Guid TenantId, string Status) : DomainEvent;
