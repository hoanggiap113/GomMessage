namespace GomMessage.Domain.Events;

public sealed record MessageCreatedDomainEvent(Guid MessageId, Guid TenantId, Guid ConversationId, string SenderType) : DomainEvent;
