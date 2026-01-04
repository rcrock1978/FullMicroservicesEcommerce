namespace ProductService.Contracts.DTOs;

public record CreateReviewDto(
    int ProductId,
    int Rating,
    string Title,
    string Comment
);
