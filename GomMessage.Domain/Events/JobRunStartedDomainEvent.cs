namespace GomMessage.Domain.Events;

public sealed record JobRunStartedDomainEvent(Guid JobRunId, Guid JobId, Guid TenantId) : DomainEvent;
