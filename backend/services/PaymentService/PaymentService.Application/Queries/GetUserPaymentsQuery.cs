using MediatR;
using PaymentService.Contracts.Dtos;
using Shared.Common.Application;

namespace PaymentService.Application.Queries;

public record GetUserPaymentsQuery(int UserId) : IRequest<Result<IEnumerable<PaymentDto>>>;
