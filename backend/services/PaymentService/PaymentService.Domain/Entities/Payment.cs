using PaymentService.Domain.Enums;
using PaymentService.Domain.Events;
using PaymentService.Domain.ValueObjects;
using Shared.Common.Domain;

namespace PaymentService.Domain.Entities;

public class Payment : BaseEntity
{
    public int UserId { get; private set; }
    public int OrderId { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public Money Amount { get; private set; } = null!;
    public PaymentStatus Status { get; private set; }
    public PaymentType PaymentType { get; private set; }
    
    // Stripe-specific properties
    public string? StripePaymentIntentId { get; private set; }
    public string? StripeChargeId { get; private set; }
    public string? StripeCustomerId { get; private set; }
    public string? CardLastFourDigits { get; private set; }
    public string? CardBrand { get; private set; }
    
    // Payment details
    public string? FailureReason { get; private set; }
    public string? FailureCode { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public DateTime? RefundedAt { get; private set; }
    public Money? RefundedAmount { get; private set; }
    public string? RefundReason { get; private set; }
    
    // Metadata
    public string? Description { get; private set; }
    public Dictionary<string, string> Metadata { get; private set; } = new();

    private Payment() { } // EF Core

    public Payment(
        int userId,
        int orderId,
        string orderNumber,
        decimal amount,
        string currency,
        PaymentType paymentType,
        string? description = null)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
        
        if (orderId <= 0)
            throw new ArgumentException("Order ID must be greater than zero.", nameof(orderId));
        
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new ArgumentException("Order number cannot be empty.", nameof(orderNumber));
        
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        UserId = userId;
        OrderId = orderId;
        OrderNumber = orderNumber;
        Amount = new Money(amount, currency);
        PaymentType = paymentType;
        Status = PaymentStatus.Pending;
        Description = description;

        AddDomainEvent(new PaymentInitiatedEvent(Id, UserId, OrderId, Amount.Amount));
    }

    public void SetStripePaymentIntent(string paymentIntentId, string? customerId = null)
    {
        if (string.IsNullOrWhiteSpace(paymentIntentId))
            throw new ArgumentException("Payment intent ID cannot be empty.", nameof(paymentIntentId));

        StripePaymentIntentId = paymentIntentId;
        if (!string.IsNullOrWhiteSpace(customerId))
            StripeCustomerId = customerId;
        
        Status = PaymentStatus.Processing;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete(
        string? chargeId = null,
        string? cardLastFour = null,
        string? cardBrand = null)
    {
        if (Status != PaymentStatus.Processing)
            throw new InvalidOperationException($"Cannot complete payment with status {Status}");

        Status = PaymentStatus.Completed;
        ProcessedAt = DateTime.UtcNow;
        StripeChargeId = chargeId;
        CardLastFourDigits = cardLastFour;
        CardBrand = cardBrand;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new PaymentCompletedEvent(Id, UserId, OrderId, Amount.Amount, ProcessedAt.Value));
    }

    public void Fail(string failureReason, string? failureCode = null)
    {
        if (Status == PaymentStatus.Completed)
            throw new InvalidOperationException("Cannot fail a completed payment");

        Status = PaymentStatus.Failed;
        FailureReason = failureReason;
        FailureCode = failureCode;
        ProcessedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new PaymentFailedEvent(Id, UserId, OrderId, FailureReason));
    }

    public void Refund(decimal refundAmount, string? reason = null)
    {
        if (Status != PaymentStatus.Completed)
            throw new InvalidOperationException("Can only refund completed payments");

        if (refundAmount <= 0)
            throw new ArgumentException("Refund amount must be greater than zero.", nameof(refundAmount));

        if (refundAmount > Amount.Amount)
            throw new ArgumentException("Refund amount cannot exceed payment amount.", nameof(refundAmount));

        Status = PaymentStatus.Refunded;
        RefundedAmount = new Money(refundAmount, Amount.Currency);
        RefundedAt = DateTime.UtcNow;
        RefundReason = reason;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new PaymentRefundedEvent(Id, UserId, OrderId, refundAmount, RefundedAt.Value));
    }

    public void Cancel(string? reason = null)
    {
        if (Status == PaymentStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed payment");

        if (Status == PaymentStatus.Refunded)
            throw new InvalidOperationException("Cannot cancel a refunded payment");

        Status = PaymentStatus.Cancelled;
        FailureReason = reason ?? "Payment cancelled";
        ProcessedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddMetadata(string key, string value)
    {
        Metadata[key] = value;
        UpdatedAt = DateTime.UtcNow;
    }
}
