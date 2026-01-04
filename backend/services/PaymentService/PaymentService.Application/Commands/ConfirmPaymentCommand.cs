using MediatR;
using PaymentService.Contracts.Dtos;
using Shared.Common.Application;

namespace PaymentService.Application.Commands;

public record ConfirmPaymentCommand(
    int PaymentId
) : IRequest<Result<PaymentDto>>;
