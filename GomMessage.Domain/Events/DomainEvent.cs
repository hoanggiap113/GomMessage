using GomMessage.Domain.Common;

namespace GomMessage.Domain.Events;

public abstract record DomainEvent : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; init; } = DateTimeOffset.UtcNow;
}
