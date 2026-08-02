using GomMessage.Domain.Common;
using GomMessage.Domain.ValueObjects;

namespace GomMessage.Domain.Entities;

public sealed class Agent : AuditableEntity
{
    public Guid TenantId { get; private set; }
    public string? ExternalId { get; private set; }
    public string? Name { get; private set; }
    public EmailAddress? Email { get; private set; }
    public string? AvatarUrl { get; private set; }
    public JsonContent? Metadata { get; private set; }

    private Agent()
    {
    }

    private Agent(Guid id, Guid tenantId, string? externalId, string? name, EmailAddress? email, string? avatarUrl, JsonContent? metadata) : base(id)
    {
        TenantId = Guard.AgainstEmptyGuid(tenantId, nameof(tenantId));
        ExternalId = string.IsNullOrWhiteSpace(externalId) ? null : externalId.Trim();
        Name = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
        Email = email;
        AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? null : avatarUrl.Trim();
        Metadata = metadata;
        MarkCreated();
    }

    public static Agent Create(Guid tenantId, string? externalId, string? name, string? email = null, string? avatarUrl = null, string? metadataJson = null)
    {
        return new Agent(
            Guid.NewGuid(),
            tenantId,
            externalId,
            name,
            string.IsNullOrWhiteSpace(email) ? null : EmailAddress.Create(email),
            avatarUrl,
            JsonContent.CreateOrNull(metadataJson));
    }

    public void UpdateProfile(string? name, string? email, string? avatarUrl)
    {
        Name = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : EmailAddress.Create(email);
        AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? null : avatarUrl.Trim();
        Touch();
    }

    public void UpdateMetadata(string? metadataJson)
    {
        Metadata = JsonContent.CreateOrNull(metadataJson);
        Touch();
    }
}
