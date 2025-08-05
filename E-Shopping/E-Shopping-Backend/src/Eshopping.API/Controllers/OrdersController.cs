using MediatR;
using AutoMapper;
using Eshopping.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Eshopping.API.ViewModels;
using Eshopping.API.Application.Commands;
using Eshopping.API.Application.Queries.OrderQueries;
namespace Eshopping.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync([FromBody] List<OrderItemCreateModel> orderItems)
    => Ok(await mediator.Send(new OrderCreateCommand() { OrderItems = mapper.Map<List<OrderItemModel>>(orderItems) }));

    [HttpGet]
    public async Task<IActionResult> GetOrdersAsync() 
    => Ok(await mediator.Send(new OrderQuery()));

    [HttpPut("{id:Guid}/cancel")]
    public async Task<IActionResult> CancelOrderAsync(Guid id) 
    => Ok(await mediator.Send(new OrderCancelCommand() { Id = id, RequestType = OrderRequest.Cancel.ToString() }));

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteOrderAsync(Guid id) 
    => Ok(await mediator.Send(new OrderDeleteCommand() { Id = id, RequestType = OrderRequest.Delete.ToString() }));
}