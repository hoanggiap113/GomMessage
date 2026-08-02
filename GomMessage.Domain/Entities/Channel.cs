using GomMessage.Domain.Common;
using GomMessage.Domain.Entities.Enums;
using GomMessage.Domain.Events;
using GomMessage.Domain.Exceptions;
using GomMessage.Domain.ValueObjects;

namespace GomMessage.Domain.Entities;

public sealed class Channel : AuditableEntity
{
    public Guid TenantId { get; private set; }
    public ChannelType ChannelType { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? ExternalId { get; private set; }
    public string? CredentialsEncrypted { get; private set; }
    public bool Active { get; private set; }
    public DateTimeOffset? LastSyncAt { get; private set; }
    public SyncStatus? LastSyncStatus { get; private set; }
    public string? LastSyncError { get; private set; }
    public JsonContent? Metadata { get; private set; }

    private Channel()
    {
    }

    private Channel(Guid id, Guid tenantId, ChannelType channelType, string name, string? externalId, string? credentialsEncrypted, JsonContent? metadata) : base(id)
    {
        TenantId = Guard.AgainstEmptyGuid(tenantId, nameof(tenantId));
        ChannelType = channelType;
        Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        ExternalId = string.IsNullOrWhiteSpace(externalId) ? null : externalId.Trim();
        CredentialsEncrypted = string.IsNullOrWhiteSpace(credentialsEncrypted) ? null : credentialsEncrypted;
        Active = true;
        Metadata = metadata;
        MarkCreated();
    }

    public static Channel Connect(Guid tenantId, ChannelType channelType, string name, string? externalId = null, string? credentialsEncrypted = null, string? metadataJson = null)
    {
        var channel = new Channel(Guid.NewGuid(), tenantId, channelType, name, externalId, credentialsEncrypted, JsonContent.CreateOrNull(metadataJson));
        channel.AddDomainEvent(new ChannelConnectedDomainEvent(channel.Id, channel.TenantId, channel.ChannelType.ToString(), channel.Name));
        return channel;
    }

    public void Rename(string name)
    {
        Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Touch();
    }

    public void UpdateCredentials(string? credentialsEncrypted)
    {
        CredentialsEncrypted = string.IsNullOrWhiteSpace(credentialsEncrypted) ? null : credentialsEncrypted;
        Touch();
    }

    public void Activate()
    {
        Active = true;
        Touch();
    }

    public void Deactivate()
    {
        Active = false;
        Touch();
    }

    public void MarkSyncStarted()
    {
        if (LastSyncStatus == SyncStatus.InProgress)
        {
            throw new DomainException(ErrorCode.ChannelSyncInProgress);
        }

        LastSyncStatus = SyncStatus.InProgress;
        LastSyncAt = DateTimeOffset.UtcNow;
        LastSyncError = null;
        Touch();
        AddDomainEvent(new ChannelSyncStatusChangedDomainEvent(Id, TenantId, LastSyncStatus.Value.ToString()));
    }

    public void MarkSyncSucceeded(DateTimeOffset? syncedAt = null)
    {
        LastSyncStatus = SyncStatus.Success;
        LastSyncAt = syncedAt ?? DateTimeOffset.UtcNow;
        LastSyncError = null;
        Touch();
        AddDomainEvent(new ChannelSyncStatusChangedDomainEvent(Id, TenantId, LastSyncStatus.Value.ToString()));
    }

    public void MarkSyncFailed(string errorMessage)
    {
        LastSyncStatus = SyncStatus.Failed;
        LastSyncAt = DateTimeOffset.UtcNow;
        LastSyncError = Guard.AgainstNullOrWhiteSpace(errorMessage, nameof(errorMessage));
        Touch();
        AddDomainEvent(new ChannelSyncStatusChangedDomainEvent(Id, TenantId, LastSyncStatus.Value.ToString()));
    }

    public void UpdateMetadata(string? metadataJson)
    {
        Metadata = JsonContent.CreateOrNull(metadataJson);
        Touch();
    }
}
