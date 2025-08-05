using System.Reflection;
using Microsoft.AspNetCore.Mvc;
namespace Eshopping.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthChecksController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult IsWorking() => Ok($"Running: {Assembly.GetEntryAssembly()!.GetName().Version};");
}