using MediatR;
using Microsoft.AspNetCore.Mvc;
using Eshopping.API.Application.Queries.AppUserQueries;
namespace Eshopping.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] bool isDeleted) => Ok(await mediator.Send(new AppUserQuery()
    {
        IsDeleted = isDeleted
    }));
}