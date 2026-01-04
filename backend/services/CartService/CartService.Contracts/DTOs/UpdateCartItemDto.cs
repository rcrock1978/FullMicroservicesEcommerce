namespace CartService.Contracts.DTOs;

public record UpdateCartItemDto
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}
