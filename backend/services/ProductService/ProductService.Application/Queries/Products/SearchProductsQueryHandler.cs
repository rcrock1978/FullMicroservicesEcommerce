using AutoMapper;
using MediatR;
using ProductService.Contracts.DTOs;
using ProductService.Domain.Repositories;
using Shared.Common.Application;

namespace ProductService.Application.Queries.Products;

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, Result<List<ProductDto>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public SearchProductsQueryHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<ProductDto>>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.SearchAsync(request.SearchTerm, cancellationToken);
        var productDtos = _mapper.Map<List<ProductDto>>(products);
        
        return Result<List<ProductDto>>.Success(productDtos);
    }
}
