namespace GomMessage.Domain.Events;

public sealed record ChannelConnectedDomainEvent(Guid ChannelId, Guid TenantId, string ChannelType, string Name) : DomainEvent;
