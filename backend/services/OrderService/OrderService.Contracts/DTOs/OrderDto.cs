using OrderService.Domain.Enums;

namespace OrderService.Contracts.DTOs;

public record OrderDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public OrderStatus Status { get; init; }
    public List<OrderItemDto> Items { get; init; } = new();
    public AddressDto ShippingAddress { get; init; } = null!;
    public PaymentMethodDto PaymentMethod { get; init; } = null!;
    public decimal SubtotalAmount { get; init; }
    public decimal ShippingCost { get; init; }
    public decimal TaxAmount { get; init; }
    public decimal TotalAmount { get; init; }
    public string Currency { get; init; } = "USD";
    public string? Notes { get; init; }
    public DateTime? CancelledAt { get; init; }
    public string? CancellationReason { get; init; }
    public DateTime? ShippedAt { get; init; }
    public DateTime? DeliveredAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public record OrderItemDto
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public decimal TotalPrice { get; init; }
    public string? ImageUrl { get; init; }
}

public record AddressDto
{
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
}

public record PaymentMethodDto
{
    public PaymentType Type { get; init; }
    public string? CardLastFourDigits { get; init; }
    public string? PaymentIntentId { get; init; }
}
