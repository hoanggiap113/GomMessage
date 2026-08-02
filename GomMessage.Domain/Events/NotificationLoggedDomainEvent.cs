namespace GomMessage.Domain.Events;

public sealed record NotificationLoggedDomainEvent(Guid NotificationLogId, Guid TenantId, string Channel, string Status) : DomainEvent;
