using MediatR;
using PaymentService.Contracts.Dtos;
using Shared.Common.Application;

namespace PaymentService.Application.Commands;

public record InitiatePaymentCommand(
    int UserId,
    CreatePaymentDto CreatePaymentDto
) : IRequest<Result<PaymentDto>>;
