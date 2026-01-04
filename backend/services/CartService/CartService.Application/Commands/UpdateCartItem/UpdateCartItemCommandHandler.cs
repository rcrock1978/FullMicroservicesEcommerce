using AutoMapper;
using CartService.Contracts.DTOs;
using CartService.Domain.Exceptions;
using CartService.Domain.Repositories;
using MediatR;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace CartService.Application.Commands.UpdateCartItem;

public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCartItemCommandHandler(
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdWithItemsAsync(request.UserId, cancellationToken);

        if (cart == null)
            return Result<CartDto>.Failure(new CartNotFoundException(request.UserId));

        if (cart.IsExpired())
            return Result<CartDto>.Failure(new CartExpiredException());

        if (!cart.HasItem(request.ProductId))
            return Result<CartDto>.Failure(new CartItemNotFoundException(request.ProductId));

        cart.UpdateItemQuantity(request.ProductId, request.Quantity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var cartDto = _mapper.Map<CartDto>(cart);
        return Result<CartDto>.Success(cartDto);
    }
}
