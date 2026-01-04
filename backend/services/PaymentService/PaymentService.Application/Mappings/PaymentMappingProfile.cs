using AutoMapper;
using PaymentService.Contracts.Dtos;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Mappings;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Amount.Currency))
            .ForMember(dest => dest.RefundedAmount, opt => opt.MapFrom(src => src.RefundedAmount != null ? src.RefundedAmount.Amount : (decimal?)null));
    }
}
