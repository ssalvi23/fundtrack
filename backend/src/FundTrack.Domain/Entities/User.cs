using FundTrack.Domain.Common;
using FundTrack.Domain.Enums;

namespace FundTrack.Domain.Entities;

public sealed class User : AuditableEntity, ISoftDeletable
{
    private User()
    {
    }

    public string Email { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; }

    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedAtUtc { get; private set; }
    public string? DeletedBy { get; private set; }

    public static User Register(string email, string fullName, string passwordHash, UserRole role = UserRole.User)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new DomainException("Full name is required.");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new DomainException("Password hash is required.");
        }

        return new User
        {
            Email = email.Trim().ToLowerInvariant(),
            FullName = fullName.Trim(),
            PasswordHash = passwordHash,
            Role = role,
        };
    }
}
