using PaymentService.Domain.Enums;
using PaymentService.Domain.Events;
using PaymentService.Domain.ValueObjects;
using Shared.Common.Domain;
using Shared.Common.Domain.Exceptions;

namespace PaymentService.Domain.Entities;

public class Payment : AggregateRoot
{
    public int OrderId { get; private set; }
    public int UserId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public string? StripePaymentIntentId { get; private set; }
    public string? StripeChargeId { get; private set; }
    public string? FailureReason { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public DateTime? RefundedAt { get; private set; }
    public Money? RefundedAmount { get; private set; }
    public string? RefundReason { get; private set; }

    private Payment() { } // For EF Core

    public Payment(int orderId, int userId, Money amount, PaymentMethod paymentMethod)
    {
        if (orderId <= 0)
            throw new DomainValidationException("OrderId must be greater than 0");
        
        if (userId <= 0)
            throw new DomainValidationException("UserId must be greater than 0");
        
        if (amount == null)
            throw new DomainValidationException("Amount is required");

        if (paymentMethod == null)
            throw new DomainValidationException("PaymentMethod is required");

        OrderId = orderId;
        UserId = userId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Status = PaymentStatus.Pending;

        AddDomainEvent(new PaymentInitiatedEvent(Id, OrderId, UserId, Amount.Amount, Amount.Currency));
    }

    public void MarkAsProcessing(string stripePaymentIntentId)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException($"Cannot mark payment as processing. Current status: {Status}");

        if (string.IsNullOrWhiteSpace(stripePaymentIntentId))
            throw new DomainValidationException("Stripe payment intent ID is required");

        Status = PaymentStatus.Processing;
        StripePaymentIntentId = stripePaymentIntentId;
    }

    public void MarkAsSucceeded(string? stripeChargeId = null)
    {
        if (Status != PaymentStatus.Processing && Status != PaymentStatus.Pending)
            throw new InvalidOperationException($"Cannot mark payment as succeeded. Current status: {Status}");

        Status = PaymentStatus.Succeeded;
        ProcessedAt = DateTime.UtcNow;
        StripeChargeId = stripeChargeId;

        AddDomainEvent(new PaymentSucceededEvent(Id, OrderId, UserId, Amount.Amount, Amount.Currency, ProcessedAt.Value));
    }

    public void MarkAsFailed(string failureReason)
    {
        if (Status == PaymentStatus.Succeeded || Status == PaymentStatus.Refunded)
            throw new InvalidOperationException($"Cannot mark payment as failed. Current status: {Status}");

        if (string.IsNullOrWhiteSpace(failureReason))
            throw new DomainValidationException("Failure reason is required");

        Status = PaymentStatus.Failed;
        FailureReason = failureReason;
        ProcessedAt = DateTime.UtcNow;

        AddDomainEvent(new PaymentFailedEvent(Id, OrderId, UserId, failureReason));
    }

    public void Refund(Money refundAmount, string refundReason)
    {
        if (Status != PaymentStatus.Succeeded)
            throw new InvalidOperationException($"Can only refund succeeded payments. Current status: {Status}");

        if (refundAmount == null)
            throw new DomainValidationException("Refund amount is required");

        if (refundAmount.Amount > Amount.Amount)
            throw new DomainValidationException("Refund amount cannot exceed payment amount");

        if (string.IsNullOrWhiteSpace(refundReason))
            throw new DomainValidationException("Refund reason is required");

        Status = PaymentStatus.Refunded;
        RefundedAmount = refundAmount;
        RefundReason = refundReason;
        RefundedAt = DateTime.UtcNow;

        AddDomainEvent(new PaymentRefundedEvent(Id, OrderId, refundAmount.Amount, refundReason, RefundedAt.Value));
    }

    public void Cancel()
    {
        if (Status == PaymentStatus.Succeeded || Status == PaymentStatus.Refunded)
            throw new InvalidOperationException($"Cannot cancel payment. Current status: {Status}");

        Status = PaymentStatus.Cancelled;
    }
}
