using AutoMapper;
using Eshopping.API.ViewModels;
using Eshopping.Domain.Aggregates.UserAggregate;
namespace Eshopping.API.Configuration.Modules.AutoMapper;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<AppUser, AppUserViewModel>()
            .ForMember(des => des.Id, e => e.MapFrom(src => src.ID))
            .ForMember(des => des.Firstname, e => e.MapFrom(src => src.FirstName))
            .ForMember(des => des.Lastname, e => e.MapFrom(src => src.LastName))
            .ForMember(des => des.Email, e => e.MapFrom(src => src.Username));
    }
}