using Shared.Common.Domain.Exceptions;

namespace Shared.Common.Domain.ValueObjects;

/// <summary>
/// Address value object
/// </summary>
public sealed class Address : IEquatable<Address>
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string PostalCode { get; }
    public string Country { get; }

    private Address(string street, string city, string state, string postalCode, string country)
    {
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
    }

    public static Address Create(string street, string city, string state, string postalCode, string country)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new DomainValidationException("Street is required");

        if (string.IsNullOrWhiteSpace(city))
            throw new DomainValidationException("City is required");

        if (string.IsNullOrWhiteSpace(state))
            throw new DomainValidationException("State is required");

        if (string.IsNullOrWhiteSpace(postalCode))
            throw new DomainValidationException("Postal code is required");

        if (string.IsNullOrWhiteSpace(country))
            throw new DomainValidationException("Country is required");

        if (street.Length > 200)
            throw new DomainValidationException("Street cannot exceed 200 characters");

        if (city.Length > 100)
            throw new DomainValidationException("City cannot exceed 100 characters");

        if (state.Length > 100)
            throw new DomainValidationException("State cannot exceed 100 characters");

        if (postalCode.Length > 20)
            throw new DomainValidationException("Postal code cannot exceed 20 characters");

        if (country.Length > 100)
            throw new DomainValidationException("Country cannot exceed 100 characters");

        return new Address(
            street.Trim(),
            city.Trim(),
            state.Trim(),
            postalCode.Trim().ToUpperInvariant(),
            country.Trim()
        );
    }

    public string GetFullAddress()
    {
        return $"{Street}, {City}, {State} {PostalCode}, {Country}";
    }

    public bool Equals(Address? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Street == other.Street &&
               City == other.City &&
               State == other.State &&
               PostalCode == other.PostalCode &&
               Country == other.Country;
    }

    public override bool Equals(object? obj)
    {
        return obj is Address address && Equals(address);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, State, PostalCode, Country);
    }

    public override string ToString() => GetFullAddress();

    public static bool operator ==(Address? left, Address? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Address? left, Address? right)
    {
        return !Equals(left, right);
    }
}
