using System.Text.RegularExpressions;
using Shared.Common.Domain.Exceptions;

namespace Shared.Common.Domain.ValueObjects;

/// <summary>
/// Email value object
/// </summary>
public sealed class Email : IEquatable<Email>
{
    private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private static readonly Regex EmailRegex = new(EmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainValidationException("Email cannot be empty");
        }

        email = email.Trim().ToLowerInvariant();

        if (email.Length > 256)
        {
            throw new DomainValidationException("Email cannot exceed 256 characters");
        }

        if (!EmailRegex.IsMatch(email))
        {
            throw new DomainValidationException($"'{email}' is not a valid email address");
        }

        return new Email(email);
    }

    public bool Equals(Email? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Email email && Equals(email);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;

    public static bool operator ==(Email? left, Email? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Email? left, Email? right)
    {
        return !Equals(left, right);
    }
}
