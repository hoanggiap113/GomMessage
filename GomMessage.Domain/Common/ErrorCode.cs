namespace GomMessage.Domain.Common;

public sealed record ErrorCode(string Code, string DefaultMessage)
{
    // Auth
    public static readonly ErrorCode EmailExists = new("EMAIL_EXISTS", "Email already exists");
    public static readonly ErrorCode EmailNotVerified = new("EMAIL_NOT_VERIFIED", "Email not verified. Please check your inbox.");
    public static readonly ErrorCode EmailAlreadyVerified = new("EMAIL_ALREADY_VERIFIED", "Email already verified");
    public static readonly ErrorCode InvalidCredentials = new("INVALID_CREDENTIALS", "Invalid email or password");

    // Account status
    public static readonly ErrorCode AccountLocked = new("ACCOUNT_LOCKED", "Your account has been locked. Please contact support.");
    public static readonly ErrorCode AccountDeleted = new("ACCOUNT_DELETED", "This account no longer exists");

    // OTP
    public static readonly ErrorCode OtpInvalid = new("OTP_INVALID", "Incorrect OTP. {remainingAttempts} attempt(s) left.");
    public static readonly ErrorCode OtpExpired = new("OTP_EXPIRED", "OTP has expired. Please request a new one.");
    public static readonly ErrorCode OtpMaxAttempts = new("OTP_MAX_ATTEMPTS", "Too many failed attempts. Please request a new OTP.");
    public static readonly ErrorCode OtpResendLimit = new("OTP_RESEND_LIMIT", "Resend limit reached. Please try again later.");
    public static readonly ErrorCode OtpResendTooSoon = new("OTP_RESEND_TOO_SOON", "Please wait before requesting a new OTP.");

    // Common
    public static readonly ErrorCode ValidationError = new("VALIDATION_ERROR", "Invalid request data");
    public static readonly ErrorCode NotFound = new("NOT_FOUND", "Resource not found");
    public static readonly ErrorCode Forbidden = new("FORBIDDEN", "Access denied");
    public static readonly ErrorCode InternalError = new("INTERNAL_ERROR", "An unexpected error occurred");
    public static readonly ErrorCode InvalidRequest = new("INVALID_REQUEST", "Request invalid");
    public static readonly ErrorCode InvalidApiKey = new("INVALID_API_KEY", "Api key invalid or null");

    // Token
    public static readonly ErrorCode RefreshTokenInvalid = new("REFRESH_TOKEN_INVALID", "Invalid refresh token");
    public static readonly ErrorCode RefreshTokenNotFound = new("REFRESH_TOKEN_NOT_FOUND", "Refresh token not found or has been revoked");
    public static readonly ErrorCode RefreshTokenExpired = new("REFRESH_TOKEN_EXPIRED", "Refresh token has expired. Please log in again.");

    // User
    public static readonly ErrorCode UserNotFound = new("USER_NOT_FOUND", "User not found");
    public static readonly ErrorCode Unauthorized = new("UNAUTHORIZED", "Authentication required");

    // Tenant
    public static readonly ErrorCode TenantNotFound = new("TENANT_NOT_FOUND", "Tenant not found");
    public static readonly ErrorCode TenantSlugExists = new("TENANT_SLUG_EXISTS", "Tenant slug already exists");
    public static readonly ErrorCode TenantNameExists = new("TENANT_NAME_EXISTS", "Tenant name already exists");
    public static readonly ErrorCode AlreadyMember = new("ALREADY_MEMBER", "User is already a member of this tenant");
    public static readonly ErrorCode InsufficientPermissions = new("INSUFFICIENT_PERMISSIONS", "You do not have permission to perform this action");
    public static readonly ErrorCode CannotRemoveOwner = new("CANNOT_REMOVE_OWNER", "Cannot remove the owner of a tenant");
    public static readonly ErrorCode MemberNotFound = new("MEMBER_NOT_FOUND", "Member not found in this tenant");
    public static readonly ErrorCode CannotLeaveAsOwner = new("CANNOT_LEAVE_AS_OWNER", "Owner cannot leave the tenant. Transfer ownership first.");

    // Invitation
    public static readonly ErrorCode InvitationNotFound = new("INVITATION_NOT_FOUND", "Invitation not found");
    public static readonly ErrorCode InvitationExpired = new("INVITATION_EXPIRED", "Invitation has expired");
    public static readonly ErrorCode InvitationAlreadyUsed = new("INVITATION_ALREADY_USED", "Invitation has already been used");
    public static readonly ErrorCode InvitationPendingExists = new("INVITATION_PENDING_EXISTS", "A pending invitation already exists for this email");

    // Channel
    public static readonly ErrorCode ChannelNotFound = new("CHANNEL_NOT_FOUND", "Channel not found");
    public static readonly ErrorCode ChannelAlreadyExists = new("CHANNEL_ALREADY_EXISTS", "This page is connected");
    public static readonly ErrorCode ChannelInvalidCredentials = new("CHANNEL_INVALID_CREDENTIALS", "Invalid or expired credentials");
    public static readonly ErrorCode ChannelSyncInProgress = new("CHANNEL_SYNC_IN_PROGRESS", "Channel is currently syncing");
    public static readonly ErrorCode ChannelAccessDenied = new("CHANNEL_ACCESS_DENIED", "No permissions are granted to operate on this channel");
}
