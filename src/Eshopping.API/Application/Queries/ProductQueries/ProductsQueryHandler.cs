using MediatR;
using AutoMapper;
using Eshopping.Infrastructure;
using Eshopping.API.ViewModels;
using Microsoft.EntityFrameworkCore;
using Eshopping.Domain.Aggregates.ProductAggregate;
namespace Eshopping.API.Application.Queries.ProductQueries;

public class ProductQuery : IRequest<List<ProductViewModel>>
{
    public bool IsDeleted { get; set; } = false;
}

public class AppUserQueryHandler(EshoppingContext eshoppingContext, IMapper mapper) : IRequestHandler<ProductQuery, List<ProductViewModel>>
{
    private readonly EshoppingContext _eshoppingContext = eshoppingContext ?? throw new ArgumentNullException(nameof(eshoppingContext));

    public async Task<List<ProductViewModel>> Handle(ProductQuery request, CancellationToken cancellationToken)
    {
        List<Product> products = await _eshoppingContext.Products
                                                        .Include(p => p.ProductCategory)
                                                        .Where(p => p.IsDeleted == request.IsDeleted)
                                                        .ToListAsync(cancellationToken);

        return mapper.Map<List<ProductViewModel>>(products);
    }
}