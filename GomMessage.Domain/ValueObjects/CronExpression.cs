using GomMessage.Domain.Exceptions;

namespace GomMessage.Domain.ValueObjects;

public sealed class CronExpression : ValueObject
{
    public string Value { get; }

    private CronExpression(string value)
    {
        Value = value;
    }

    public static CronExpression Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Cron expression must not be empty.");
        }

        var normalized = value.Trim();
        var partCount = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        if (partCount is < 5 or > 6)
        {
            throw new DomainException("Cron expression must have 5 or 6 parts.");
        }

        return new CronExpression(normalized);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
