namespace ProductService.Domain.Exceptions;

public class CategoryNotFoundException : Exception
{
    public int CategoryId { get; }

    public CategoryNotFoundException(int categoryId)
        : base($"Category with ID {categoryId} was not found.")
    {
        CategoryId = categoryId;
    }
}
