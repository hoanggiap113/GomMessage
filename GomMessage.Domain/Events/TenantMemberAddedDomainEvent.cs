namespace GomMessage.Domain.Events;

public sealed record TenantMemberAddedDomainEvent(Guid TenantId, Guid UserId, string Role) : DomainEvent;
