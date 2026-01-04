using CartService.Domain.Exceptions;
using CartService.Domain.Repositories;
using MediatR;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace CartService.Application.Commands.RemoveFromCart;

public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, Result>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveFromCartCommandHandler(
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdWithItemsAsync(request.UserId, cancellationToken);

        if (cart == null)
            return Result.Failure(new CartNotFoundException(request.UserId).Message);

        if (cart.IsExpired())
            return Result.Failure(new CartExpiredException().Message);

        cart.RemoveItem(request.ProductId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
