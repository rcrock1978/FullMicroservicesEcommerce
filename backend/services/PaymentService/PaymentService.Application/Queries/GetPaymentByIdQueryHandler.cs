using AutoMapper;
using MediatR;
using PaymentService.Contracts.Dtos;
using PaymentService.Domain.Exceptions;
using PaymentService.Domain.Repositories;
using Shared.Common.Application;

namespace PaymentService.Application.Queries;

public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Result<PaymentDto>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<Result<PaymentDto>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken);
        
        if (payment == null)
        {
            return Result<PaymentDto>.Failure(new PaymentNotFoundException(request.PaymentId).Message);
        }

        var paymentDto = _mapper.Map<PaymentDto>(payment);
        return Result<PaymentDto>.Success(paymentDto);
    }
}
