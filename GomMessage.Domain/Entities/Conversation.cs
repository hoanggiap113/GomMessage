using GomMessage.Domain.Common;
using GomMessage.Domain.Events;
using GomMessage.Domain.ValueObjects;

namespace GomMessage.Domain.Entities;

public sealed class Conversation : AuditableEntity
{
    public Guid TenantId { get; private set; }
    public Guid ChannelId { get; private set; }
    public string? ExternalConversationId { get; private set; }
    public string? ExternalUserId { get; private set; }
    public string? CustomerName { get; private set; }
    public DateTimeOffset? LastMessageAt { get; private set; }
    public int MessageCount { get; private set; }
    public JsonContent? Metadata { get; private set; }

    private Conversation()
    {
    }

    private Conversation(Guid id, Guid tenantId, Guid channelId, string? externalConversationId, string? externalUserId, string? customerName, JsonContent? metadata) : base(id)
    {
        TenantId = Guard.AgainstEmptyGuid(tenantId, nameof(tenantId));
        ChannelId = Guard.AgainstEmptyGuid(channelId, nameof(channelId));
        ExternalConversationId = string.IsNullOrWhiteSpace(externalConversationId) ? null : externalConversationId.Trim();
        ExternalUserId = string.IsNullOrWhiteSpace(externalUserId) ? null : externalUserId.Trim();
        CustomerName = string.IsNullOrWhiteSpace(customerName) ? null : customerName.Trim();
        MessageCount = 0;
        Metadata = metadata;
        MarkCreated();
    }

    public static Conversation Create(Guid tenantId, Guid channelId, string? externalConversationId, string? externalUserId, string? customerName, string? metadataJson = null)
    {
        var conversation = new Conversation(Guid.NewGuid(), tenantId, channelId, externalConversationId, externalUserId, customerName, JsonContent.CreateOrNull(metadataJson));
        conversation.AddDomainEvent(new ConversationCreatedDomainEvent(conversation.Id, conversation.TenantId, conversation.ChannelId, conversation.ExternalConversationId));
        return conversation;
    }

    public void UpdateCustomer(string? externalUserId, string? customerName)
    {
        ExternalUserId = string.IsNullOrWhiteSpace(externalUserId) ? null : externalUserId.Trim();
        CustomerName = string.IsNullOrWhiteSpace(customerName) ? null : customerName.Trim();
        Touch();
    }

    public void RegisterMessage(DateTimeOffset? sentAt)
    {
        MessageCount++;
        LastMessageAt = sentAt ?? DateTimeOffset.UtcNow;
        Touch();
    }

    public void UpdateMetadata(string? metadataJson)
    {
        Metadata = JsonContent.CreateOrNull(metadataJson);
        Touch();
    }
}
