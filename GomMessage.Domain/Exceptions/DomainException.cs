using GomMessage.Domain.Common;

namespace GomMessage.Domain.Exceptions;

public class DomainException : Exception
{
    public ErrorCode? ErrorCode { get; }
    public IReadOnlyDictionary<string, object> Metadata { get; }

    public DomainException(string message) : base(message)
    {
        Metadata = new Dictionary<string, object>();
    }

    public DomainException(ErrorCode errorCode) : base(errorCode.DefaultMessage)
    {
        ErrorCode = errorCode;
        Metadata = new Dictionary<string, object>();
    }

    public DomainException(ErrorCode errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
        Metadata = new Dictionary<string, object>();
    }

    public DomainException(ErrorCode errorCode, string message, IReadOnlyDictionary<string, object> metadata) : base(message)
    {
        ErrorCode = errorCode;
        Metadata = metadata;
    }

}
