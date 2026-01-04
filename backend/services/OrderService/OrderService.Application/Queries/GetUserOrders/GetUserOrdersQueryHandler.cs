using AutoMapper;
using OrderService.Contracts.DTOs;
using OrderService.Domain.Repositories;
using MediatR;
using Shared.Common.Application;

namespace OrderService.Application.Queries.GetUserOrders;

public class GetUserOrdersQueryHandler : IRequestHandler<GetUserOrdersQuery, Result<List<OrderDto>>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetUserOrdersQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<OrderDto>>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByUserIdWithItemsAsync(request.UserId, cancellationToken);
        var orderDtos = _mapper.Map<List<OrderDto>>(orders);
        return Result<List<OrderDto>>.Success(orderDtos);
    }
}
