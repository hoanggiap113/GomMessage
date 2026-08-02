using GomMessage.Domain.Common;

namespace GomMessage.Domain.Entities;

public sealed class AppSettings : AuditableEntity
{
    public Guid TenantId { get; private set; }
    public string SettingKey { get; private set; } = string.Empty;
    public string? ValueEncrypted { get; private set; }
    public string? ValuePlain { get; private set; }

    private AppSettings()
    {
    }

    private AppSettings(Guid id, Guid tenantId, string settingKey, string? valueEncrypted, string? valuePlain) : base(id)
    {
        TenantId = Guard.AgainstEmptyGuid(tenantId, nameof(tenantId));
        SettingKey = Guard.AgainstNullOrWhiteSpace(settingKey, nameof(settingKey));
        ValueEncrypted = string.IsNullOrWhiteSpace(valueEncrypted) ? null : valueEncrypted;
        ValuePlain = string.IsNullOrWhiteSpace(valuePlain) ? null : valuePlain;
        MarkCreated();
    }

    public static AppSettings Create(Guid tenantId, string settingKey, string? valueEncrypted = null, string? valuePlain = null)
    {
        return new AppSettings(Guid.NewGuid(), tenantId, settingKey, valueEncrypted, valuePlain);
    }

    public void SetEncryptedValue(string? valueEncrypted)
    {
        ValueEncrypted = string.IsNullOrWhiteSpace(valueEncrypted) ? null : valueEncrypted;
        Touch();
    }

    public void SetPlainValue(string? valuePlain)
    {
        ValuePlain = string.IsNullOrWhiteSpace(valuePlain) ? null : valuePlain;
        Touch();
    }
}
