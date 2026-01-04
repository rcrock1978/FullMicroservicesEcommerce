namespace CartService.Contracts.DTOs;

public record CartDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public List<CartItemDto> Items { get; init; } = new();
    public decimal SubtotalAmount { get; init; }
    public string Currency { get; init; } = "USD";
    public int TotalItems { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public record CartItemDto
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public decimal TotalPrice { get; init; }
    public string Currency { get; init; } = "USD";
    public string? ImageUrl { get; init; }
}
