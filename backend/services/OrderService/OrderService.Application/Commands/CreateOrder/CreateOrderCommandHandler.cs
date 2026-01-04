using AutoMapper;
using OrderService.Contracts.DTOs;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using OrderService.Domain.ValueObjects;
using MediatR;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace OrderService.Application.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderNumber = GenerateOrderNumber();
        
        var address = new Address(
            request.ShippingAddress.Street,
            request.ShippingAddress.City,
            request.ShippingAddress.State,
            request.ShippingAddress.PostalCode,
            request.ShippingAddress.Country);

        var paymentMethod = new PaymentMethod(
            request.PaymentMethod.Type,
            request.PaymentMethod.CardLastFourDigits,
            request.PaymentMethod.PaymentIntentId);

        var order = new Order(
            request.UserId,
            orderNumber,
            address,
            paymentMethod,
            new Money(request.ShippingCost),
            request.Notes);

        foreach (var item in request.Items)
        {
            order.AddItem(
                item.ProductId,
                item.ProductName,
                item.UnitPrice,
                item.Quantity,
                item.ImageUrl);
        }

        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var orderDto = _mapper.Map<OrderDto>(order);
        return Result<OrderDto>.Success(orderDto);
    }

    private static string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
    }
}
