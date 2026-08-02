using GomMessage.Domain.Exceptions;

namespace GomMessage.Domain.Common;

public static class Guard
{
    public static string AgainstNullOrWhiteSpace(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"'{parameterName}' must not be empty.");
        }

        return value.Trim();
    }

    public static Guid AgainstEmptyGuid(Guid value, string parameterName)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException($"'{parameterName}' must not be empty.");
        }

        return value;
    }

    public static DateTimeOffset AgainstPastDate(DateTimeOffset value, string parameterName)
    {
        if (value <= DateTimeOffset.UtcNow)
        {
            throw new DomainException($"'{parameterName}' must be in the future.");
        }

        return value;
    }
}
