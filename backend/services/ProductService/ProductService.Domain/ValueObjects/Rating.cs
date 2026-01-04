using Shared.Common.Domain;

namespace ProductService.Domain.ValueObjects;

public sealed class Rating : ValueObject
{
    public decimal Value { get; private set; }

    private Rating() { } // EF Core

    public Rating(decimal value)
    {
        if (value < 0 || value > 5)
            throw new ArgumentException("Rating must be between 0 and 5", nameof(value));

        Value = Math.Round(value, 2);
    }

    public static Rating Create(int totalRating, int reviewCount)
    {
        if (reviewCount == 0)
            return new Rating(0);

        return new Rating((decimal)totalRating / reviewCount);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => $"{Value:F1}";

    public static implicit operator decimal(Rating rating) => rating.Value;
}
