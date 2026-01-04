using OrderService.Domain.Enums;

namespace OrderService.Contracts.DTOs;

public record CreateOrderDto
{
    public List<OrderItemRequestDto> Items { get; init; } = new();
    public AddressDto ShippingAddress { get; init; } = null!;
    public PaymentMethodDto PaymentMethod { get; init; } = null!;
    public decimal ShippingCost { get; init; }
    public string? Notes { get; init; }
}

public record OrderItemRequestDto
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public string? ImageUrl { get; init; }
}
