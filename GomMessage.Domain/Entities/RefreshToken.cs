using GomMessage.Domain.Common;

namespace GomMessage.Domain.Entities;

public sealed class RefreshToken : AggregateRoot
{
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; private set; }
    public bool Revoked { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private RefreshToken()
    {
    }

    private RefreshToken(Guid id, Guid userId, string tokenHash, DateTimeOffset expiresAt) : base(id)
    {
        UserId = Guard.AgainstEmptyGuid(userId, nameof(userId));
        TokenHash = Guard.AgainstNullOrWhiteSpace(tokenHash, nameof(tokenHash));
        ExpiresAt = Guard.AgainstPastDate(expiresAt, nameof(expiresAt));
        Revoked = false;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static RefreshToken Create(Guid userId, string tokenHash, DateTimeOffset expiresAt)
    {
        return new RefreshToken(Guid.NewGuid(), userId, tokenHash, expiresAt);
    }

    public bool IsExpired(DateTimeOffset? now = null) => ExpiresAt <= (now ?? DateTimeOffset.UtcNow);

    public bool IsActive(DateTimeOffset? now = null) => !Revoked && !IsExpired(now);

    public void Revoke()
    {
        Revoked = true;
    }
}
