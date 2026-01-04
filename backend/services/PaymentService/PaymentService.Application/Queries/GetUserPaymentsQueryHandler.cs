using AutoMapper;
using MediatR;
using PaymentService.Contracts.Dtos;
using PaymentService.Domain.Repositories;
using Shared.Common.Application;

namespace PaymentService.Application.Queries;

public class GetUserPaymentsQueryHandler : IRequestHandler<GetUserPaymentsQuery, Result<IEnumerable<PaymentDto>>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public GetUserPaymentsQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<PaymentDto>>> Handle(GetUserPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        var paymentDtos = _mapper.Map<IEnumerable<PaymentDto>>(payments);
        return Result<IEnumerable<PaymentDto>>.Success(paymentDtos);
    }
}
