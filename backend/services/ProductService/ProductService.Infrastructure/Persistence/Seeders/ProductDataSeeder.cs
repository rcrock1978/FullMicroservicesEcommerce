using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;
using ProductService.Domain.ValueObjects;

namespace ProductService.Infrastructure.Persistence.Seeders;

public static class ProductDataSeeder
{
    public static async Task SeedAsync(ProductDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return; // Data already seeded

        // Seed Categories
        var electronics = new Category("Electronics", "Electronic devices and accessories", "electronics", null, 1);
        var clothing = new Category("Clothing", "Fashion and apparel", "clothing", null, 2);
        var books = new Category("Books", "Books and literature", "books", null, 3);

        context.Categories.AddRange(electronics, clothing, books);
        await context.SaveChangesAsync();

        // Seed Subcategories
        var laptops = new Category("Laptops", "Portable computers", "laptops", electronics.Id, 1);
        var smartphones = new Category("Smartphones", "Mobile phones", "smartphones", electronics.Id, 2);
        var menClothing = new Category("Men's Clothing", "Clothing for men", "mens-clothing", clothing.Id, 1);
        var womenClothing = new Category("Women's Clothing", "Clothing for women", "womens-clothing", clothing.Id, 2);

        context.Categories.AddRange(laptops, smartphones, menClothing, womenClothing);
        await context.SaveChangesAsync();

        // Seed Products
        var products = new[]
        {
            new Product(
                "MacBook Pro 16\"",
                "Apple MacBook Pro 16-inch with M3 Max chip, 32GB RAM, 1TB SSD",
                new Sku("MBP-16-M3-32-1TB"),
                new Money(2999.99m, "USD"),
                laptops.Id
            ),
            new Product(
                "Dell XPS 15",
                "Dell XPS 15 laptop with Intel i7, 16GB RAM, 512GB SSD",
                new Sku("DELL-XPS15-I7-16"),
                new Money(1799.99m, "USD"),
                laptops.Id
            ),
            new Product(
                "iPhone 15 Pro",
                "Apple iPhone 15 Pro with A17 Pro chip, 256GB storage",
                new Sku("IPHONE-15-PRO-256"),
                new Money(1199.99m, "USD"),
                smartphones.Id
            ),
            new Product(
                "Samsung Galaxy S24",
                "Samsung Galaxy S24 with Snapdragon 8 Gen 3, 256GB storage",
                new Sku("SAMSUNG-S24-256"),
                new Money(999.99m, "USD"),
                smartphones.Id
            ),
            new Product(
                "Men's Casual Shirt",
                "Cotton casual shirt for men, available in multiple colors",
                new Sku("MENS-SHIRT-001"),
                new Money(49.99m, "USD"),
                menClothing.Id
            ),
            new Product(
                "Women's Summer Dress",
                "Light and comfortable summer dress for women",
                new Sku("WOMENS-DRESS-001"),
                new Money(79.99m, "USD"),
                womenClothing.Id
            ),
            new Product(
                "The Great Gatsby",
                "Classic novel by F. Scott Fitzgerald",
                new Sku("BOOK-GATSBY-001"),
                new Money(12.99m, "USD"),
                books.Id
            ),
            new Product(
                "Clean Code",
                "A Handbook of Agile Software Craftsmanship by Robert C. Martin",
                new Sku("BOOK-CLEAN-CODE"),
                new Money(39.99m, "USD"),
                books.Id
            )
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        // Update product statuses
        foreach (var product in products)
        {
            product.ChangeStatus(ProductStatus.Active);
        }

        // Seed Inventory
        var inventories = new[]
        {
            new Inventory(products[0].Id, 50, 10, 20),
            new Inventory(products[1].Id, 75, 15, 30),
            new Inventory(products[2].Id, 100, 20, 50),
            new Inventory(products[3].Id, 85, 15, 40),
            new Inventory(products[4].Id, 200, 25, 100),
            new Inventory(products[5].Id, 150, 20, 75),
            new Inventory(products[6].Id, 500, 50, 200),
            new Inventory(products[7].Id, 300, 40, 150)
        };

        context.Inventories.AddRange(inventories);
        await context.SaveChangesAsync();

        // Seed Product Images
        var images = new List<ProductImage>
        {
            new ProductImage("https://example.com/images/macbook-pro-1.jpg", "MacBook Pro 16 inch", true, 1),
            new ProductImage("https://example.com/images/macbook-pro-2.jpg", "MacBook Pro side view", false, 2),
            new ProductImage("https://example.com/images/dell-xps-1.jpg", "Dell XPS 15", true, 1),
            new ProductImage("https://example.com/images/iphone-15-pro-1.jpg", "iPhone 15 Pro", true, 1),
            new ProductImage("https://example.com/images/samsung-s24-1.jpg", "Samsung Galaxy S24", true, 1),
            new ProductImage("https://example.com/images/mens-shirt-1.jpg", "Men's Casual Shirt", true, 1),
            new ProductImage("https://example.com/images/womens-dress-1.jpg", "Women's Summer Dress", true, 1),
            new ProductImage("https://example.com/images/gatsby-1.jpg", "The Great Gatsby cover", true, 1),
            new ProductImage("https://example.com/images/clean-code-1.jpg", "Clean Code book cover", true, 1)
        };

        // Add images to products
        for (int i = 0; i < products.Length; i++)
        {
            if (i < images.Count)
            {
                products[i].AddImage(images[i].Url, images[i].AltText, images[i].IsPrimary);
            }
        }

        await context.SaveChangesAsync();

        // Seed Reviews
        var reviews = new[]
        {
            new Review(products[0].Id, 1, 5, "Excellent laptop", "Best laptop I've ever owned. Performance is outstanding!", false),
            new Review(products[0].Id, 2, 4, "Great but expensive", "Amazing laptop but the price is quite steep.", true),
            new Review(products[2].Id, 3, 5, "Love it!", "The camera quality is incredible. Highly recommend!", true),
            new Review(products[3].Id, 4, 4, "Good phone", "Solid phone with great features. Battery life could be better.", true),
            new Review(products[7].Id, 5, 5, "Must-read for developers", "Every developer should read this book. Changed how I write code.", false)
        };

        context.Reviews.AddRange(reviews);
        await context.SaveChangesAsync();

        // Update product ratings
        products[0].AddReview(reviews[0]);
        products[0].AddReview(reviews[1]);
        products[2].AddReview(reviews[2]);
        products[3].AddReview(reviews[3]);
        products[7].AddReview(reviews[4]);

        await context.SaveChangesAsync();
    }
}
