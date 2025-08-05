using MediatR;
using Microsoft.AspNetCore.Mvc;
using Eshopping.API.Application.Queries.ProductsQueries;
namespace Eshopping.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] bool isDeleted) => Ok(await mediator.Send(new ProductQuery()
    {
        IsDeleted = isDeleted
    }));
}