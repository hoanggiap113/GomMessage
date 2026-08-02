using GomMessage.Domain.Common;

namespace GomMessage.Domain.Exceptions;

public sealed class BadRequestException : DomainException
{
    public BadRequestException(ErrorCode errorCode) : base(errorCode)
    {
    }

    public BadRequestException(ErrorCode errorCode, string message) : base(errorCode, message)
    {
    }

    public BadRequestException(ErrorCode errorCode, string message, IReadOnlyDictionary<string, object> metadata)
        : base(errorCode, message, metadata)
    {
    }
}
