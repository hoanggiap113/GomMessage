using GomMessage.Domain.Common;
using GomMessage.Domain.Entities.Enums;
using GomMessage.Domain.Events;
using GomMessage.Domain.Exceptions;
using GomMessage.Domain.ValueObjects;

namespace GomMessage.Domain.Entities;

public sealed class User : AuditableEntity
{
    public EmailAddress Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = string.Empty;
    public string? Name { get; private set; }
    public string? Telephone { get; private set; }
    public UserStatus Status { get; private set; }
    public int TokenVersion { get; private set; }
    private User()
    {
    }

    private User(Guid id, EmailAddress email, string passwordHash, string? name, string? telephone, UserStatus status) : base(id)
    {
        Email = email;
        PasswordHash = Guard.AgainstNullOrWhiteSpace(passwordHash, nameof(passwordHash));
        Name = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
        Telephone = string.IsNullOrWhiteSpace(telephone) ? null : telephone.Trim();
        Status = status;
        TokenVersion = 0;
        MarkCreated();
    }

    public static User Register(string email, string passwordHash, string? name = null,string? telephone = null)
    {
        var user = new User(Guid.NewGuid(), EmailAddress.Create(email), passwordHash, name,telephone, UserStatus.Pending);
        user.AddDomainEvent(new UserRegisteredDomainEvent(user.Id, user.Email.Value));
        return user;
    }

    public void Activate()
    {
        EnsureNotDeleted();
        Status = UserStatus.Active;
        Touch();
        AddDomainEvent(new UserStatusChangedDomainEvent(Id, Status.ToString()));
    }

    public void Lock()
    {
        EnsureNotDeleted();
        Status = UserStatus.Locked;
        Touch();
        AddDomainEvent(new UserStatusChangedDomainEvent(Id, Status.ToString()));
    }

    public void Delete()
    {
        Status = UserStatus.Deleted;
        Touch();
        AddDomainEvent(new UserStatusChangedDomainEvent(Id, Status.ToString()));
    }

    public void ChangeName(string? name)
    {
        EnsureNotDeleted();
        Name = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
        Touch();
    }

    public void ChangePasswordHash(string passwordHash)
    {
        EnsureNotDeleted();
        PasswordHash = Guard.AgainstNullOrWhiteSpace(passwordHash, nameof(passwordHash));
        IncreaseTokenVersion();
        Touch();
    }

    public void IncreaseTokenVersion()
    {
        TokenVersion++;
        Touch();
    }

    private void EnsureNotDeleted()
    {
        if (Status == UserStatus.Deleted)
        {
            throw new DomainException(ErrorCode.AccountDeleted);
        }
    }
}
