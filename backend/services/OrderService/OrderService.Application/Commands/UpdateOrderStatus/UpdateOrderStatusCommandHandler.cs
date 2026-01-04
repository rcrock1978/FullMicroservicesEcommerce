using OrderService.Domain.Enums;
using OrderService.Domain.Repositories;
using MediatR;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace OrderService.Application.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Result>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderStatusCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order == null)
            return Result.Failure($"Order with id {request.OrderId} not found");

        try
        {
            switch (request.NewStatus)
            {
                case OrderStatus.Confirmed:
                    order.ConfirmOrder();
                    break;
                case OrderStatus.Processing:
                    order.StartProcessing();
                    break;
                case OrderStatus.Shipped:
                    order.Ship();
                    break;
                case OrderStatus.Delivered:
                    order.Deliver();
                    break;
                default:
                    return Result.Failure($"Cannot update to status {request.NewStatus}");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}
