using OrderService.Contracts.DTOs;
using MediatR;
using Shared.Common.Application;

namespace OrderService.Application.Commands.CreateOrder;

public record CreateOrderCommand : IRequest<Result<OrderDto>>
{
    public int UserId { get; init; }
    public List<OrderItemRequestDto> Items { get; init; } = new();
    public AddressDto ShippingAddress { get; init; } = null!;
    public PaymentMethodDto PaymentMethod { get; init; } = null!;
    public decimal ShippingCost { get; init; }
    public string? Notes { get; init; }
}
