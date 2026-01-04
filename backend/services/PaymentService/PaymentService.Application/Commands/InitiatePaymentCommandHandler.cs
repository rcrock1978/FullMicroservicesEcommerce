using AutoMapper;
using MediatR;
using PaymentService.Application.Services;
using PaymentService.Contracts.Dtos;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace PaymentService.Application.Commands;

public class InitiatePaymentCommandHandler : IRequestHandler<InitiatePaymentCommand, Result<PaymentDto>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IStripeService _stripeService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InitiatePaymentCommandHandler(
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

    public async Task<Result<PaymentDto>> Handle(InitiatePaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if payment already exists for this order
            var existingPayment = await _paymentRepository.GetByOrderIdAsync(request.CreatePaymentDto.OrderId, cancellationToken);
            if (existingPayment != null)
            {
                return Result<PaymentDto>.Failure("Payment already exists for this order");
            }

            // Create payment entity
            var payment = new Payment(
                request.UserId,
                request.CreatePaymentDto.OrderId,
                request.CreatePaymentDto.OrderNumber,
                request.CreatePaymentDto.Amount,
                request.CreatePaymentDto.Currency,
                request.CreatePaymentDto.PaymentType,
                request.CreatePaymentDto.Description
            );

            // Create Stripe PaymentIntent
            var paymentIntentId = await _stripeService.CreatePaymentIntentAsync(
                payment.Amount.Amount,
                payment.Amount.Currency,
                request.CreatePaymentDto.OrderNumber,
                cancellationToken);

            payment.SetStripePaymentIntent(paymentIntentId);

            await _paymentRepository.AddAsync(payment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var paymentDto = _mapper.Map<PaymentDto>(payment);
            return Result<PaymentDto>.Success(paymentDto);
        }
        catch (Exception ex)
        {
            return Result<PaymentDto>.Failure($"Failed to initiate payment: {ex.Message}");
        }
    }
}
