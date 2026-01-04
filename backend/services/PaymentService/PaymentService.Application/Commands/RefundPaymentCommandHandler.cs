using AutoMapper;
using MediatR;
using PaymentService.Application.Services;
using PaymentService.Contracts.Dtos;
using PaymentService.Domain.Exceptions;
using PaymentService.Domain.Repositories;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace PaymentService.Application.Commands;

public class RefundPaymentCommandHandler : IRequestHandler<RefundPaymentCommand, Result<PaymentDto>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IStripeService _stripeService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RefundPaymentCommandHandler(
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

    public async Task<Result<PaymentDto>> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken);
            if (payment == null)
            {
                return Result<PaymentDto>.Failure(new PaymentNotFoundException(request.PaymentId).Message);
            }

            if (string.IsNullOrEmpty(payment.StripeChargeId))
            {
                return Result<PaymentDto>.Failure("Cannot refund payment without charge ID");
            }

            // Process refund with Stripe
            await _stripeService.RefundPaymentAsync(
                payment.StripeChargeId,
                request.RefundDto.Amount,
                cancellationToken);

            payment.Refund(request.RefundDto.Amount, request.RefundDto.Reason);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var paymentDto = _mapper.Map<PaymentDto>(payment);
            return Result<PaymentDto>.Success(paymentDto);
        }
        catch (InvalidOperationException ex)
        {
            return Result<PaymentDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result<PaymentDto>.Failure($"Failed to refund payment: {ex.Message}");
        }
    }
}
