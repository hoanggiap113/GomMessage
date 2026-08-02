using GomMessage.Domain.Common;

namespace GomMessage.Domain.Exceptions;

public sealed class NotFoundException : DomainException
{
    public NotFoundException(ErrorCode errorCode) : base(errorCode)
    {
    }

    public NotFoundException(ErrorCode errorCode, string message) : base(errorCode, message)
    {
    }
}
