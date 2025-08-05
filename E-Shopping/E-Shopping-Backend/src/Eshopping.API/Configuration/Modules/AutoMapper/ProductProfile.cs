using AutoMapper;
using Eshopping.API.ViewModels;
using Eshopping.Domain.Aggregates.ProductAggregate;
namespace Eshopping.API.Configuration.Modules.AutoMapper;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductViewModel>()
            .ForMember(des => des.Id, e => e.MapFrom(src => src.ID))
            .ForMember(des => des.CategoryName, e => e.MapFrom(src => src.ProductCategory.Name));
    }
}