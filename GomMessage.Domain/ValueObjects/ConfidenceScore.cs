using GomMessage.Domain.Exceptions;

namespace GomMessage.Domain.ValueObjects;

public sealed class ConfidenceScore : ValueObject
{
    public double Value { get; }

    private ConfidenceScore(double value)
    {
        Value = value;
    }

    public static ConfidenceScore Create(double value)
    {
        if (double.IsNaN(value) || value < 0 || value > 1)
        {
            throw new DomainException("Confidence score must be between 0 and 1.");
        }

        return new ConfidenceScore(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
