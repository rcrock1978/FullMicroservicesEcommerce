using AutoMapper;
using MediatR;
using PaymentService.Application.Services;
using PaymentService.Contracts.Dtos;
using PaymentService.Domain.Exceptions;
using PaymentService.Domain.Repositories;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace PaymentService.Application.Commands;

public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, Result<PaymentDto>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IStripeService _stripeService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ConfirmPaymentCommandHandler(
        IPaymentRepository paymentRepository,
        IStripeService stripeService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _stripeService = stripeService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaymentDto>> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken);
            if (payment == null)
            {
                return Result<PaymentDto>.Failure(new PaymentNotFoundException(request.PaymentId).Message);
            }

            if (string.IsNullOrEmpty(payment.StripePaymentIntentId))
            {
                return Result<PaymentDto>.Failure("Payment intent not found");
            }

            // Confirm payment with Stripe
            var (chargeId, cardLastFour, cardBrand) = await _stripeService.ConfirmPaymentAsync(
                payment.StripePaymentIntentId,
                cancellationToken);

            payment.Complete(chargeId, cardLastFour, cardBrand);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var paymentDto = _mapper.Map<PaymentDto>(payment);
            return Result<PaymentDto>.Success(paymentDto);
        }
        catch (Exception ex)
        {
            return Result<PaymentDto>.Failure($"Failed to confirm payment: {ex.Message}");
        }
    }
}
