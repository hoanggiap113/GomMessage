using System.Text.Json;
using GomMessage.Domain.Exceptions;

namespace GomMessage.Domain.ValueObjects;

public sealed class JsonContent : ValueObject
{
    public string Value { get; }

    private JsonContent(string value)
    {
        Value = value;
    }

    public static JsonContent Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Json content must not be empty.");
        }

        try
        {
            using var _ = JsonDocument.Parse(value);
            return new JsonContent(value.Trim());
        }
        catch (JsonException ex)
        {
            throw new DomainException($"Json content is invalid: {ex.Message}");
        }
    }

    public static JsonContent? CreateOrNull(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : Create(value);
    }

    public override string ToString() => Value;

    public static implicit operator string(JsonContent jsonContent) => jsonContent.Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
