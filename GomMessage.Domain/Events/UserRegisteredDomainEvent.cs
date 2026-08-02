namespace GomMessage.Domain.Events;

public sealed record UserRegisteredDomainEvent(Guid UserId, string Email) : DomainEvent;
