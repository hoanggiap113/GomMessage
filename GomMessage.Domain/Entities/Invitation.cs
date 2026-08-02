using GomMessage.Domain.Common;
using GomMessage.Domain.Entities.Enums;
using GomMessage.Domain.Events;
using GomMessage.Domain.Exceptions;
using GomMessage.Domain.ValueObjects;

namespace GomMessage.Domain.Entities;

public sealed class Invitation : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public EmailAddress Email { get; private set; } = null!;
    public TenantRole Role { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public InvitationStatus Status { get; private set; }
    public Guid InvitedById { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private Invitation()
    {
    }

    private Invitation(Guid id, Guid tenantId, EmailAddress email, TenantRole role, string token, Guid invitedById, DateTimeOffset expiresAt) : base(id)
    {
        TenantId = Guard.AgainstEmptyGuid(tenantId, nameof(tenantId));
        Email = email;
        Role = role;
        Token = Guard.AgainstNullOrWhiteSpace(token, nameof(token));
        Status = InvitationStatus.Pending;
        InvitedById = Guard.AgainstEmptyGuid(invitedById, nameof(invitedById));
        ExpiresAt = Guard.AgainstPastDate(expiresAt, nameof(expiresAt));
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Invitation Create(Guid tenantId, string email, TenantRole role, string token, Guid invitedById, DateTimeOffset expiresAt)
    {
        var invitation = new Invitation(Guid.NewGuid(), tenantId, EmailAddress.Create(email), role, token, invitedById, expiresAt);
        invitation.AddDomainEvent(new InvitationCreatedDomainEvent(invitation.Id, invitation.TenantId, invitation.Email.Value, invitation.ExpiresAt));
        return invitation;
    }

    public void Accept()
    {
        EnsurePending();
        if (ExpiresAt <= DateTimeOffset.UtcNow)
        {
            Expire();
            throw new DomainException(ErrorCode.InvitationExpired);
        }

        Status = InvitationStatus.Accepted;
        AddDomainEvent(new InvitationAcceptedDomainEvent(Id, TenantId, Email.Value));
    }

    public void Expire()
    {
        if (Status == InvitationStatus.Pending)
        {
            Status = InvitationStatus.Expired;
        }
    }

    public void Cancel()
    {
        EnsurePending();
        Status = InvitationStatus.Cancelled;
    }

    private void EnsurePending()
    {
        if (Status != InvitationStatus.Pending)
        {
            throw new DomainException(ErrorCode.InvitationAlreadyUsed);
        }
    }
}
