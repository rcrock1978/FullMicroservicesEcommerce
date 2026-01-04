using System.Text.RegularExpressions;
using Shared.Common.Domain.Exceptions;

namespace Shared.Common.Domain.ValueObjects;

/// <summary>
/// Phone number value object
/// </summary>
public sealed class PhoneNumber : IEquatable<PhoneNumber>
{
    // Matches international phone numbers with optional country code
    private const string PhonePattern = @"^\+?[1-9]\d{1,14}$";
    private static readonly Regex PhoneRegex = new(PhonePattern, RegexOptions.Compiled);

    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new DomainValidationException("Phone number cannot be empty");
        }

        // Remove common formatting characters
        var cleaned = Regex.Replace(phoneNumber, @"[\s\-\(\)]", "");

        if (cleaned.Length < 7 || cleaned.Length > 15)
        {
            throw new DomainValidationException("Phone number must be between 7 and 15 digits");
        }

        if (!PhoneRegex.IsMatch(cleaned))
        {
            throw new DomainValidationException($"'{phoneNumber}' is not a valid phone number");
        }

        return new PhoneNumber(cleaned);
    }

    public string GetFormatted()
    {
        // Simple formatting: +1 (XXX) XXX-XXXX for US numbers
        if (Value.StartsWith("+1") && Value.Length == 12)
        {
            return $"+1 ({Value.Substring(2, 3)}) {Value.Substring(5, 3)}-{Value.Substring(8, 4)}";
        }

        return Value;
    }

    public bool Equals(PhoneNumber? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is PhoneNumber phone && Equals(phone);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phone) => phone.Value;

    public static bool operator ==(PhoneNumber? left, PhoneNumber? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PhoneNumber? left, PhoneNumber? right)
    {
        return !Equals(left, right);
    }
}
