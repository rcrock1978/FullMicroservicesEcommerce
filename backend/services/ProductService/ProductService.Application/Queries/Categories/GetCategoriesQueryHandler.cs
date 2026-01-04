using AutoMapper;
using MediatR;
using ProductService.Contracts.DTOs;
using ProductService.Domain.Repositories;
using Shared.Common.Application;

namespace ProductService.Application.Queries.Categories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<List<CategoryDto>>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
        
        return Result<List<CategoryDto>>.Success(categoryDtos);
    }
}
