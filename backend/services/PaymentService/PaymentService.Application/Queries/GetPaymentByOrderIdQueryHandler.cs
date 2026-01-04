using AutoMapper;
using MediatR;
using PaymentService.Contracts.Dtos;
using PaymentService.Domain.Repositories;
using Shared.Common.Application;

namespace PaymentService.Application.Queries;

public class GetPaymentByOrderIdQueryHandler : IRequestHandler<GetPaymentByOrderIdQuery, Result<PaymentDto>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public GetPaymentByOrderIdQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<Result<PaymentDto>> Handle(GetPaymentByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);
        
        if (payment == null)
        {
            return Result<PaymentDto>.Failure($"Payment for order ID '{request.OrderId}' was not found");
        }

        var paymentDto = _mapper.Map<PaymentDto>(payment);
        return Result<PaymentDto>.Success(paymentDto);
    }
}
