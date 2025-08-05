using MediatR;
using AutoMapper;
using Eshopping.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Eshopping.Domain.Aggregates.UserAggregate;
using Eshopping.API.ViewModels;
namespace Eshopping.API.Application.Queries.AppUserQueries;

public class AppUserQuery : IRequest<List<AppUserViewModel>>
{
    public Guid UserId { get; set; }
    public bool IsDeleted { get; set; } = false;
}

public class AppUserQueryHandler(EshoppingContext eshoppingContext, IMapper mapper) : IRequestHandler<AppUserQuery, List<AppUserViewModel>>
{
    private readonly EshoppingContext _eshoppingContext = eshoppingContext ?? throw new ArgumentNullException(nameof(eshoppingContext));

    public async Task<List<AppUserViewModel>> Handle(AppUserQuery request, CancellationToken cancellationToken)
    {
        List<AppUser> appUsers = await _eshoppingContext.Users
                                                        .Where(user => user.IsDeleted == request.IsDeleted)
                                                        .ToListAsync(cancellationToken);
   

        return mapper.Map<List<AppUserViewModel>>(appUsers);
    }
}