using CartService.Domain.Exceptions;
using CartService.Domain.Repositories;
using MediatR;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace CartService.Application.Commands.ClearCart;

public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Result>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClearCartCommandHandler(
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdWithItemsAsync(request.UserId, cancellationToken);

        if (cart == null)
            return Result.Failure(new CartNotFoundException(request.UserId).Message);

        cart.Clear();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
