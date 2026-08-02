namespace GomMessage.Domain.Events;

public sealed record UserStatusChangedDomainEvent(Guid UserId, string Status) : DomainEvent;
