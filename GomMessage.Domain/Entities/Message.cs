using GomMessage.Domain.Common;
using GomMessage.Domain.Entities.Enums;
using GomMessage.Domain.Events;
using GomMessage.Domain.ValueObjects;

namespace GomMessage.Domain.Entities;

public sealed class Message : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public Guid ConversationId { get; private set; }
    public Guid? AgentId { get; private set; }
    public string? ExternalMessageId { get; private set; }
    public SenderType SenderType { get; private set; }
    public string? SenderName { get; private set; }
    public string? SenderExternalId { get; private set; }
    public string? Content { get; private set; }
    public MessageContentType? ContentType { get; private set; }
    public JsonContent? Attachments { get; private set; }
    public DateTimeOffset? SentAt { get; private set; }
    public JsonContent? RawData { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private Message()
    {
    }

    private Message(
        Guid id,
        Guid tenantId,
        Guid conversationId,
        Guid? agentId,
        string? externalMessageId,
        SenderType senderType,
        string? senderName,
        string? senderExternalId,
        string? content,
        MessageContentType? contentType,
        JsonContent? attachments,
        DateTimeOffset? sentAt,
        JsonContent? rawData) : base(id)
    {
        TenantId = Guard.AgainstEmptyGuid(tenantId, nameof(tenantId));
        ConversationId = Guard.AgainstEmptyGuid(conversationId, nameof(conversationId));
        AgentId = agentId == Guid.Empty ? null : agentId;
        ExternalMessageId = string.IsNullOrWhiteSpace(externalMessageId) ? null : externalMessageId.Trim();
        SenderType = senderType;
        SenderName = string.IsNullOrWhiteSpace(senderName) ? null : senderName.Trim();
        SenderExternalId = string.IsNullOrWhiteSpace(senderExternalId) ? null : senderExternalId.Trim();
        Content = string.IsNullOrWhiteSpace(content) ? null : content;
        ContentType = contentType;
        Attachments = attachments;
        SentAt = sentAt;
        RawData = rawData;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Message Create(
        Guid tenantId,
        Guid conversationId,
        SenderType senderType,
        string? content,
        Guid? agentId = null,
        string? externalMessageId = null,
        string? senderName = null,
        string? senderExternalId = null,
        MessageContentType? contentType = MessageContentType.Text,
        string? attachmentsJson = null,
        DateTimeOffset? sentAt = null,
        string? rawDataJson = null)
    {
        var message = new Message(
            Guid.NewGuid(),
            tenantId,
            conversationId,
            agentId,
            externalMessageId,
            senderType,
            senderName,
            senderExternalId,
            content,
            contentType,
            JsonContent.CreateOrNull(attachmentsJson),
            sentAt,
            JsonContent.CreateOrNull(rawDataJson));

        message.AddDomainEvent(new MessageCreatedDomainEvent(message.Id, message.TenantId, message.ConversationId, message.SenderType.ToString()));
        return message;
    }
}
