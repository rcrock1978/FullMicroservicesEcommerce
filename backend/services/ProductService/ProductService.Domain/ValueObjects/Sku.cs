using Shared.Common.Domain;

namespace ProductService.Domain.ValueObjects;

public sealed class Sku : ValueObject
{
    public string Value { get; private set; }

    private Sku() { } // EF Core

    public Sku(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SKU cannot be empty", nameof(value));

        if (value.Length < 3 || value.Length > 50)
            throw new ArgumentException("SKU must be between 3 and 50 characters", nameof(value));

        if (!value.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_'))
            throw new ArgumentException("SKU can only contain letters, numbers, hyphens, and underscores", nameof(value));

        Value = value.ToUpperInvariant();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(Sku sku) => sku.Value;
}
