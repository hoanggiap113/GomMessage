namespace GomMessage.Domain.Common;

public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
}
