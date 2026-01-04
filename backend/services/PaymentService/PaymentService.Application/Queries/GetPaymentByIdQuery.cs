using MediatR;
using PaymentService.Contracts.Dtos;
using Shared.Common.Application;

namespace PaymentService.Application.Queries;

public record GetPaymentByIdQuery(int PaymentId) : IRequest<Result<PaymentDto>>;
