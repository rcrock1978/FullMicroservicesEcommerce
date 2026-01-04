using AutoMapper;
using MediatR;
using ProductService.Contracts.DTOs;
using ProductService.Domain.Repositories;
using Shared.Common.Application;

namespace ProductService.Application.Queries.Products;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDetailDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProductDetailDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdWithDetailsAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            return Result<ProductDetailDto>.Failure($"Product with ID {request.ProductId} not found");
        }

        var productDto = _mapper.Map<ProductDetailDto>(product);
        return Result<ProductDetailDto>.Success(productDto);
    }
}
