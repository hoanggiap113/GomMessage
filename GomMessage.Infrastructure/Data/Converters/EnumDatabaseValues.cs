using GomMessage.Domain.Entities.Enums;

namespace GomMessage.Infrastructure.Data.Converters;

internal static class EnumDatabaseValues
{
    public static string ToDatabaseValue(UserStatus value) => value switch
    {
        UserStatus.Pending => "PENDING",
        UserStatus.Active => "ACTIVE",
        UserStatus.Locked => "LOCKED",
        UserStatus.Deleted => "DELETED",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static UserStatus ToUserStatus(string value) => Normalize(value) switch
    {
        "PENDING" => UserStatus.Pending,
        "ACTIVE" => UserStatus.Active,
        "LOCKED" => UserStatus.Locked,
        "DELETED" => UserStatus.Deleted,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported user status.")
    };

    public static string ToDatabaseValue(TenantRole value) => value switch
    {
        TenantRole.Owner => "OWNER",
        TenantRole.Admin => "ADMIN",
        TenantRole.Member => "MEMBER",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static TenantRole ToTenantRole(string value) => Normalize(value) switch
    {
        "OWNER" => TenantRole.Owner,
        "ADMIN" => TenantRole.Admin,
        "MEMBER" => TenantRole.Member,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported tenant role.")
    };

    public static string ToDatabaseValue(MembershipStatus value) => value switch
    {
        MembershipStatus.Pending => "PENDING",
        MembershipStatus.Active => "ACTIVE",
        MembershipStatus.Revoked => "REVOKED",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static MembershipStatus ToMembershipStatus(string value) => Normalize(value) switch
    {
        "PENDING" => MembershipStatus.Pending,
        "ACTIVE" => MembershipStatus.Active,
        "REVOKED" => MembershipStatus.Revoked,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported membership status.")
    };

    public static string ToDatabaseValue(InvitationStatus value) => value switch
    {
        InvitationStatus.Pending => "PENDING",
        InvitationStatus.Accepted => "ACCEPTED",
        InvitationStatus.Expired => "EXPIRED",
        InvitationStatus.Cancelled => "CANCELLED",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static InvitationStatus ToInvitationStatus(string value) => Normalize(value) switch
    {
        "PENDING" => InvitationStatus.Pending,
        "ACCEPTED" => InvitationStatus.Accepted,
        "EXPIRED" => InvitationStatus.Expired,
        "CANCELLED" => InvitationStatus.Cancelled,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported invitation status.")
    };

    public static string ToDatabaseValue(ChannelType value) => value switch
    {
        ChannelType.ZaloOa => "ZALO_OA",
        ChannelType.Facebook => "FACEBOOK",
        ChannelType.Pancake => "PANCAKE",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static ChannelType ToChannelType(string value) => Normalize(value) switch
    {
        "ZALO_OA" => ChannelType.ZaloOa,
        "FACEBOOK" => ChannelType.Facebook,
        "PANCAKE" => ChannelType.Pancake,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported channel type.")
    };

    public static string ToDatabaseValue(SyncStatus value) => value switch
    {
        SyncStatus.Success => "SUCCESS",
        SyncStatus.Failed => "FAILED",
        SyncStatus.InProgress => "IN_PROGRESS",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static SyncStatus ToSyncStatus(string value) => Normalize(value) switch
    {
        "SUCCESS" => SyncStatus.Success,
        "FAILED" => SyncStatus.Failed,
        "IN_PROGRESS" => SyncStatus.InProgress,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported sync status.")
    };

    public static string ToDatabaseValue(JobType value) => value switch
    {
        JobType.Qc => "QC",
        JobType.Classification => "CLASSIFICATION",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static JobType ToJobType(string value) => Normalize(value) switch
    {
        "QC" => JobType.Qc,
        "CLASSIFICATION" => JobType.Classification,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported job type.")
    };

    public static string ToDatabaseValue(JobScheduleType value) => value switch
    {
        JobScheduleType.Manual => "MANUAL",
        JobScheduleType.Cron => "CRON",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static JobScheduleType ToJobScheduleType(string value) => Normalize(value) switch
    {
        "MANUAL" => JobScheduleType.Manual,
        "CRON" => JobScheduleType.Cron,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported job schedule type.")
    };

    public static string ToDatabaseValue(JobRunStatus value) => value switch
    {
        JobRunStatus.Pending => "PENDING",
        JobRunStatus.Running => "RUNNING",
        JobRunStatus.Success => "SUCCESS",
        JobRunStatus.Failed => "FAILED",
        JobRunStatus.Partial => "PARTIAL",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static JobRunStatus ToJobRunStatus(string value) => Normalize(value) switch
    {
        "PENDING" => JobRunStatus.Pending,
        "RUNNING" => JobRunStatus.Running,
        "SUCCESS" => JobRunStatus.Success,
        "FAILED" => JobRunStatus.Failed,
        "PARTIAL" => JobRunStatus.Partial,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported job run status.")
    };

    public static string ToDatabaseValue(SeverityLevel value) => value switch
    {
        SeverityLevel.Low => "LOW",
        SeverityLevel.Medium => "MEDIUM",
        SeverityLevel.High => "HIGH",
        SeverityLevel.Critical => "CRITICAL",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static SeverityLevel ToSeverityLevel(string value) => Normalize(value) switch
    {
        "LOW" => SeverityLevel.Low,
        "MEDIUM" => SeverityLevel.Medium,
        "HIGH" => SeverityLevel.High,
        "CRITICAL" => SeverityLevel.Critical,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported severity level.")
    };

    public static string ToDatabaseValue(SenderType value) => value switch
    {
        SenderType.Agent => "AGENT",
        SenderType.Customer => "CUSTOMER",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static SenderType ToSenderType(string value) => Normalize(value) switch
    {
        "AGENT" => SenderType.Agent,
        "CUSTOMER" => SenderType.Customer,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported sender type.")
    };

    public static string ToDatabaseValue(MessageContentType value) => value switch
    {
        MessageContentType.Text => "text",
        MessageContentType.Attachment => "attachment",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static MessageContentType ToMessageContentType(string value) => value.Trim().ToLowerInvariant() switch
    {
        "text" => MessageContentType.Text,
        "attachment" => MessageContentType.Attachment,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported message content type.")
    };

    public static string ToDatabaseValue(NotificationChannel value) => value switch
    {
        NotificationChannel.Telegram => "TELEGRAM",
        NotificationChannel.Email => "EMAIL",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static NotificationChannel ToNotificationChannel(string value) => Normalize(value) switch
    {
        "TELEGRAM" => NotificationChannel.Telegram,
        "EMAIL" => NotificationChannel.Email,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported notification channel.")
    };

    public static string ToDatabaseValue(NotificationStatus value) => value switch
    {
        NotificationStatus.Sent => "SENT",
        NotificationStatus.Failed => "FAILED",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static NotificationStatus ToNotificationStatus(string value) => Normalize(value) switch
    {
        "SENT" => NotificationStatus.Sent,
        "FAILED" => NotificationStatus.Failed,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported notification status.")
    };

    private static string Normalize(string value)
        => value.Trim().Replace('-', '_').Replace(' ', '_').ToUpperInvariant();
}
