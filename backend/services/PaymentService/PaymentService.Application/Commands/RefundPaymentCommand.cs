using MediatR;
using PaymentService.Contracts.Dtos;
using Shared.Common.Application;

namespace PaymentService.Application.Commands;

public record RefundPaymentCommand(
    int PaymentId,
    RefundPaymentDto RefundDto
) : IRequest<Result<PaymentDto>>;
