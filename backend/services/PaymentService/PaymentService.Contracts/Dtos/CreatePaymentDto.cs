using PaymentService.Domain.Enums;

namespace PaymentService.Contracts.Dtos;

public record CreatePaymentDto(
    int OrderId,
    string OrderNumber,
    decimal Amount,
    string Currency,
    PaymentType PaymentType,
    string? Description
);
