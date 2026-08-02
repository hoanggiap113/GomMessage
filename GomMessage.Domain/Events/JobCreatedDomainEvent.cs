namespace GomMessage.Domain.Events;

public sealed record JobCreatedDomainEvent(Guid JobId, Guid TenantId, string JobType, string Name) : DomainEvent;
