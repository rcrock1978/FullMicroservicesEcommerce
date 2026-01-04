using Shared.Common.Domain;

namespace ProductService.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Slug { get; private set; }
    public int? ParentCategoryId { get; private set; }
    public Category ParentCategory { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }
    
    private readonly List<Category> _childCategories = new();
    public IReadOnlyCollection<Category> ChildCategories => _childCategories.AsReadOnly();
    
    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Category() { } // EF Core

    public Category(string name, string description, string slug, int? parentCategoryId = null, int displayOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Category slug cannot be empty", nameof(slug));

        Name = name;
        Description = description;
        Slug = slug.ToLowerInvariant();
        ParentCategoryId = parentCategoryId;
        DisplayOrder = displayOrder;
        IsActive = true;
    }

    public void Update(string name, string description, string slug, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty", nameof(name));

        Name = name;
        Description = description;
        Slug = slug.ToLowerInvariant();
        DisplayOrder = displayOrder;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
