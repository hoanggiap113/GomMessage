using GomMessage.Domain.Common;
using GomMessage.Domain.Events;
using GomMessage.Domain.ValueObjects;

namespace GomMessage.Domain.Entities;

public sealed class Tenant : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public TenantSlug Slug { get; private set; } = null!;
    public Guid OwnerId { get; private set; }
    public JsonContent? Settings { get; private set; }

    private Tenant()
    {
    }

    private Tenant(Guid id, string name, TenantSlug slug, Guid ownerId, JsonContent? settings) : base(id)
    {
        Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Slug = slug;
        OwnerId = Guard.AgainstEmptyGuid(ownerId, nameof(ownerId));
        Settings = settings;
        MarkCreated();
    }

    public static Tenant Create(string name, string slug, Guid ownerId, string? settingsJson = null)
    {
        var tenant = new Tenant(Guid.NewGuid(), name, TenantSlug.Create(slug), ownerId, JsonContent.CreateOrNull(settingsJson));
        tenant.AddDomainEvent(new TenantCreatedDomainEvent(tenant.Id, tenant.OwnerId, tenant.Slug.Value));
        return tenant;
    }

    public void Rename(string name)
    {
        Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Touch();
    }

    public void ChangeSlug(string slug)
    {
        Slug = TenantSlug.Create(slug);
        Touch();
    }

    public void UpdateSettings(string? settingsJson)
    {
        Settings = JsonContent.CreateOrNull(settingsJson);
        Touch();
    }

    public void TransferOwnership(Guid newOwnerId)
    {
        OwnerId = Guard.AgainstEmptyGuid(newOwnerId, nameof(newOwnerId));
        Touch();
    }
}
