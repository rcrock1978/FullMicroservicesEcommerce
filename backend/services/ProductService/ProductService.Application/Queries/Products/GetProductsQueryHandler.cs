using AutoMapper;
using MediatR;
using ProductService.Contracts.DTOs;
using ProductService.Domain.Repositories;
using Shared.Common.Application;

namespace ProductService.Application.Queries.Products;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<List<ProductDto>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var productDtos = _mapper.Map<List<ProductDto>>(products);
        
        return Result<List<ProductDto>>.Success(productDtos);
    }
}
