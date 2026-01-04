using AutoMapper;
using OrderService.Contracts.DTOs;
using OrderService.Domain.Repositories;
using MediatR;
using Shared.Common.Application;

namespace OrderService.Application.Queries.GetOrder;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId, cancellationToken);

        if (order == null)
            return Result<OrderDto>.Failure($"Order with id {request.OrderId} not found");

        var orderDto = _mapper.Map<OrderDto>(order);
        return Result<OrderDto>.Success(orderDto);
    }
}
