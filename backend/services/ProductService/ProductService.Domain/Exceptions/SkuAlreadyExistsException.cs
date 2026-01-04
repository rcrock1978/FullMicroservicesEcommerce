namespace ProductService.Domain.Exceptions;

public class SkuAlreadyExistsException : Exception
{
    public string Sku { get; }

    public SkuAlreadyExistsException(string sku)
        : base($"Product with SKU '{sku}' already exists.")
    {
        Sku = sku;
    }
}
