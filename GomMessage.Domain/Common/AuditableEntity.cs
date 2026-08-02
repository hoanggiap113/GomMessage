namespace GomMessage.Domain.Common;

public abstract class AuditableEntity : AggregateRoot
{
    public DateTimeOffset CreatedAt { get; protected set; }
    public DateTimeOffset UpdatedAt { get; protected set; }

    protected AuditableEntity()
    {
    }

    protected AuditableEntity(Guid id) : base(id)
    {
    }

    protected void MarkCreated(DateTimeOffset? at = null)
    {
        var now = at ?? DateTimeOffset.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }

    protected void Touch(DateTimeOffset? at = null)
    {
        UpdatedAt = at ?? DateTimeOffset.UtcNow;
    }
}
