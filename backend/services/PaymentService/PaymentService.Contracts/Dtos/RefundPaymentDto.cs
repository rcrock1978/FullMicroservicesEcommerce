namespace PaymentService.Contracts.Dtos;

public record RefundPaymentDto(
    decimal Amount,
    string? Reason
);
