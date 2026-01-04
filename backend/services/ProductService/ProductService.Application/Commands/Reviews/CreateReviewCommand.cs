using MediatR;
using ProductService.Contracts.DTOs;
using Shared.Common.Application;

namespace ProductService.Application.Commands.Reviews;

public record CreateReviewCommand(
    int ProductId,
    int UserId,
    int Rating,
    string Title,
    string Comment
) : IRequest<Result<ReviewDto>>;
