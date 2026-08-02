namespace GomMessage.Domain.Events;

public sealed record InvitationCreatedDomainEvent(Guid InvitationId, Guid TenantId, string Email, DateTimeOffset ExpiresAt) : DomainEvent;
