using AutoMapper;
using MediatR;
using ProductService.Contracts.DTOs;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace ProductService.Application.Commands.Reviews;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<ReviewDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateReviewCommandHandler(
        IProductRepository productRepository,
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            return Result<ReviewDto>.Failure($"Product with ID {request.ProductId} not found");
        }

        // Check if user already reviewed this product
        var existingReview = await _reviewRepository.GetByProductAndUserAsync(request.ProductId, request.UserId, cancellationToken);
        if (existingReview != null)
        {
            return Result<ReviewDto>.Failure("You have already reviewed this product");
        }

        var review = new Review(
            request.ProductId,
            request.UserId,
            request.Rating,
            request.Title,
            request.Comment);

        product.AddReview(review);
        await _reviewRepository.AddAsync(review, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var reviewDto = _mapper.Map<ReviewDto>(review);
        return Result<ReviewDto>.Success(reviewDto);
    }
}
