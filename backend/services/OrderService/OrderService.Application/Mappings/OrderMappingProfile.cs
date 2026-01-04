using AutoMapper;
using OrderService.Contracts.DTOs;
using OrderService.Domain.Entities;

namespace OrderService.Application.Mappings;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.SubtotalAmount, opt => opt.MapFrom(src => src.SubtotalAmount.Amount))
            .ForMember(dest => dest.ShippingCost, opt => opt.MapFrom(src => src.ShippingCost.Amount))
            .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.TaxAmount.Amount))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.TotalAmount.Currency))
            .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Amount))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.GetTotalPrice().Amount));

        CreateMap<Domain.ValueObjects.Address, AddressDto>();
        CreateMap<Domain.ValueObjects.PaymentMethod, PaymentMethodDto>();
    }
}
