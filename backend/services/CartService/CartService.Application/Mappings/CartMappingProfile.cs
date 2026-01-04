using AutoMapper;
using CartService.Contracts.DTOs;
using CartService.Domain.Entities;

namespace CartService.Application.Mappings;

public class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        CreateMap<Cart, CartDto>()
            .ForMember(dest => dest.SubtotalAmount, opt => opt.MapFrom(src => src.GetSubtotal().Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.GetSubtotal().Currency))
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.GetTotalItems()));

        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Amount))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.GetTotalPrice().Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.UnitPrice.Currency));
    }
}
