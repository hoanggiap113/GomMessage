namespace GomMessage.Domain.Events;

public sealed record InvitationAcceptedDomainEvent(Guid InvitationId, Guid TenantId, string Email) : DomainEvent;
