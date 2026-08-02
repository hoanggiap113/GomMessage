using GomMessage.Domain.Common;
using GomMessage.Domain.Entities.Enums;
using GomMessage.Domain.Events;

namespace GomMessage.Domain.Entities;

public sealed class UserTenant : AggregateRoot
{
    public Guid UserId { get; private set; }
    public Guid TenantId { get; private set; }
    public TenantRole Role { get; private set; }
    public MembershipStatus Status { get; private set; }
    public Guid? InvitedById { get; private set; }
    public DateTimeOffset? JoinedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private UserTenant()
    {
    }

    private UserTenant(Guid id, Guid userId, Guid tenantId, TenantRole role, MembershipStatus status, Guid? invitedById) : base(id)
    {
        UserId = Guard.AgainstEmptyGuid(userId, nameof(userId));
        TenantId = Guard.AgainstEmptyGuid(tenantId, nameof(tenantId));
        Role = role;
        Status = status;
        InvitedById = invitedById;
        JoinedAt = status == MembershipStatus.Active ? DateTimeOffset.UtcNow : null;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static UserTenant Create(Guid userId, Guid tenantId, TenantRole role, Guid? invitedById = null)
    {
        var membership = new UserTenant(Guid.NewGuid(), userId, tenantId, role, MembershipStatus.Active, invitedById);
        membership.AddDomainEvent(new TenantMemberAddedDomainEvent(tenantId, userId, role.ToString()));
        return membership;
    }

    public static UserTenant Pending(Guid userId, Guid tenantId, TenantRole role, Guid invitedById)
    {
        return new UserTenant(Guid.NewGuid(), userId, tenantId, role, MembershipStatus.Pending, invitedById);
    }

    public void Activate()
    {
        Status = MembershipStatus.Active;
        JoinedAt ??= DateTimeOffset.UtcNow;
        AddDomainEvent(new TenantMemberAddedDomainEvent(TenantId, UserId, Role.ToString()));
    }

    public void ChangeRole(TenantRole role)
    {
        Role = role;
    }

    public void Revoke()
    {
        Status = MembershipStatus.Revoked;
    }
}
