using AutoMapper;
using CartService.Contracts.DTOs;
using CartService.Domain.Repositories;
using MediatR;
using Shared.Common.Application;

namespace CartService.Application.Queries.GetCart;

public class GetCartQueryHandler : IRequestHandler<GetCartQuery, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public GetCartQueryHandler(
        ICartRepository cartRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdWithItemsAsync(request.UserId, cancellationToken);

        if (cart == null || cart.IsExpired())
        {
            // Return empty cart
            return Result<CartDto>.Success(new CartDto
            {
                UserId = request.UserId,
                Items = new List<CartItemDto>(),
                SubtotalAmount = 0,
                Currency = "USD",
                TotalItems = 0
            });
        }

        var cartDto = _mapper.Map<CartDto>(cart);
        return Result<CartDto>.Success(cartDto);
    }
}
