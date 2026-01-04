namespace ProductService.Domain.Exceptions;

public class ProductNotFoundException : Exception
{
    public int ProductId { get; }

    public ProductNotFoundException(int productId)
        : base($"Product with ID {productId} was not found.")
    {
        ProductId = productId;
    }
}
