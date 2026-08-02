namespace GomMessage.Domain.Events;

public sealed record ConversationCreatedDomainEvent(Guid ConversationId, Guid TenantId, Guid ChannelId, string? ExternalConversationId) : DomainEvent;
