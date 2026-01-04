using AutoMapper;
using CartService.Contracts.DTOs;
using CartService.Domain.Entities;
using CartService.Domain.Repositories;
using CartService.Domain.ValueObjects;
using MediatR;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace CartService.Application.Commands.AddToCart;

public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddToCartCommandHandler(
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        // Get or create cart for user
        var cart = await _cartRepository.GetByUserIdWithItemsAsync(request.UserId, cancellationToken);

        if (cart == null)
        {
            cart = new Cart(request.UserId);
            await _cartRepository.AddAsync(cart, cancellationToken);
        }
        else if (cart.IsExpired())
        {
            cart.Clear();
        }

        // Add item to cart
        cart.AddItem(
            request.ProductId,
            request.ProductName,
            request.UnitPrice,
            request.Quantity,
            request.ImageUrl);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var cartDto = _mapper.Map<CartDto>(cart);
        return Result<CartDto>.Success(cartDto);
    }
}
