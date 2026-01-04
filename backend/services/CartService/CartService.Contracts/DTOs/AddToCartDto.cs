namespace CartService.Contracts.DTOs;

public record AddToCartDto
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; } = 1;
    public string? ImageUrl { get; init; }
}
