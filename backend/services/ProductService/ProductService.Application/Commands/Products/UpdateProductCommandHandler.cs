using AutoMapper;
using MediatR;
using ProductService.Contracts.DTOs;
using ProductService.Domain.Enums;
using ProductService.Domain.Repositories;
using ProductService.Domain.ValueObjects;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace ProductService.Application.Commands.Products;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            return Result<ProductDto>.Failure($"Product with ID {request.ProductId} not found");
        }

        var price = new Money(request.Price, request.Currency);
        product.UpdateDetails(request.Name, request.Description, price);

        if (Enum.TryParse<ProductStatus>(request.Status, out var status))
        {
            product.ChangeStatus(status);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var productDto = _mapper.Map<ProductDto>(product);
        return Result<ProductDto>.Success(productDto);
    }
}
