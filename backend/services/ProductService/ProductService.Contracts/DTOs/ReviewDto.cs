namespace ProductService.Contracts.DTOs;

public record ReviewDto(
    int Id,
    int ProductId,
    int UserId,
    int Rating,
    string Title,
    string Comment,
    bool IsVerifiedPurchase,
    DateTime ReviewedAt
);
