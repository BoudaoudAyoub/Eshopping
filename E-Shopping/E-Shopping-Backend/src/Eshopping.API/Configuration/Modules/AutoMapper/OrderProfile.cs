using AutoMapper;
using Eshopping.API.ViewModels;
using Eshopping.API.Application.Commands;
namespace Eshopping.API.Configuration.Modules.AutoMapper;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderItemCreateModel, OrderItemModel>();
    }
}