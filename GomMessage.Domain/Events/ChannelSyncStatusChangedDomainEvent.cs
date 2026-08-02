namespace GomMessage.Domain.Events;

public sealed record ChannelSyncStatusChangedDomainEvent(Guid ChannelId, Guid TenantId, string SyncStatus) : DomainEvent;
