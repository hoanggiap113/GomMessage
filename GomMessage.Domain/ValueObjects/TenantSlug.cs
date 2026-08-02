using System.Text.RegularExpressions;
using GomMessage.Domain.Exceptions;

namespace GomMessage.Domain.ValueObjects;

public sealed class TenantSlug : ValueObject
{
    private static readonly Regex SlugRegex = new(
        @"^[a-z0-9]+(?:-[a-z0-9]+)*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public string Value { get; }

    private TenantSlug(string value)
    {
        Value = value;
    }

    public static TenantSlug Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Tenant slug must not be empty.");
        }

        var normalized = value.Trim().ToLowerInvariant();
        if (!SlugRegex.IsMatch(normalized))
        {
            throw new DomainException("Tenant slug can contain lowercase letters, numbers and hyphens only.");
        }

        return new TenantSlug(normalized);
    }

    public override string ToString() => Value;

    public static implicit operator string(TenantSlug slug) => slug.Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
