using GomMessage.Domain.Common;
using GomMessage.Domain.Entities.Enums;
using GomMessage.Domain.Events;

namespace GomMessage.Domain.Entities;

public sealed class NotificationLog : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public Guid? JobId { get; private set; }
    public Guid? JobRunId { get; private set; }
    public NotificationChannel Channel { get; private set; }
    public string? Recipient { get; private set; }
    public string? Subject { get; private set; }
    public string? Body { get; private set; }
    public NotificationStatus Status { get; private set; }
    public string? ErrorMessage { get; private set; }
    public DateTimeOffset? SentAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private NotificationLog()
    {
    }

    private NotificationLog(Guid id, Guid tenantId, Guid? jobId, Guid? jobRunId, NotificationChannel channel, string? recipient, string? subject, string? body, NotificationStatus status, string? errorMessage, DateTimeOffset? sentAt) : base(id)
    {
        TenantId = Guard.AgainstEmptyGuid(tenantId, nameof(tenantId));
        JobId = jobId == Guid.Empty ? null : jobId;
        JobRunId = jobRunId == Guid.Empty ? null : jobRunId;
        Channel = channel;
        Recipient = string.IsNullOrWhiteSpace(recipient) ? null : recipient.Trim();
        Subject = string.IsNullOrWhiteSpace(subject) ? null : subject.Trim();
        Body = string.IsNullOrWhiteSpace(body) ? null : body;
        Status = status;
        ErrorMessage = string.IsNullOrWhiteSpace(errorMessage) ? null : errorMessage;
        SentAt = sentAt;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static NotificationLog Sent(Guid tenantId, NotificationChannel channel, string? recipient, string? subject, string? body, Guid? jobId = null, Guid? jobRunId = null, DateTimeOffset? sentAt = null)
    {
        var log = new NotificationLog(Guid.NewGuid(), tenantId, jobId, jobRunId, channel, recipient, subject, body, NotificationStatus.Sent, null, sentAt ?? DateTimeOffset.UtcNow);
        log.AddDomainEvent(new NotificationLoggedDomainEvent(log.Id, log.TenantId, log.Channel.ToString(), log.Status.ToString()));
        return log;
    }

    public static NotificationLog Failed(Guid tenantId, NotificationChannel channel, string? recipient, string? subject, string? body, string errorMessage, Guid? jobId = null, Guid? jobRunId = null)
    {
        var log = new NotificationLog(Guid.NewGuid(), tenantId, jobId, jobRunId, channel, recipient, subject, body, NotificationStatus.Failed, Guard.AgainstNullOrWhiteSpace(errorMessage, nameof(errorMessage)), null);
        log.AddDomainEvent(new NotificationLoggedDomainEvent(log.Id, log.TenantId, log.Channel.ToString(), log.Status.ToString()));
        return log;
    }
}
