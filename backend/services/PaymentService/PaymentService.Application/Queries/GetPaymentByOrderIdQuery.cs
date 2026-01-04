using MediatR;
using PaymentService.Contracts.Dtos;
using Shared.Common.Application;

namespace PaymentService.Application.Queries;

public record GetPaymentByOrderIdQuery(int OrderId) : IRequest<Result<PaymentDto>>;
