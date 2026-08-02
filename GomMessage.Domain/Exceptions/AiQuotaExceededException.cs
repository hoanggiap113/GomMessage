namespace GomMessage.Domain.Exceptions;

public sealed class AiQuotaExceededException : DomainException
{
    public AiQuotaExceededException(string message) : base(message)
    {
    }
}
