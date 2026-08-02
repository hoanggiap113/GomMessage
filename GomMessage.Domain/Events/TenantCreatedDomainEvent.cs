namespace GomMessage.Domain.Events;

public sealed record TenantCreatedDomainEvent(Guid TenantId, Guid OwnerId, string Slug) : DomainEvent;
