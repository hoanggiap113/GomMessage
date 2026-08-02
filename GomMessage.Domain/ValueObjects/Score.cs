using GomMessage.Domain.Exceptions;

namespace GomMessage.Domain.ValueObjects;

public sealed class Score : ValueObject
{
    public double Value { get; }

    private Score(double value)
    {
        Value = value;
    }

    public static Score Create(double value)
    {
        if (double.IsNaN(value) || value < 0 || value > 100)
        {
            throw new DomainException("Score must be between 0 and 100.");
        }

        return new Score(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
