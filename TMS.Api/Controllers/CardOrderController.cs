using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.CardOrders.Commands.Create;
using TMS.Application.CardOrders.Commands.UpdateCardOrderCommand;
using TMS.Application.CardOrders.Queries.Get;
using TMS.Application.CardOrders.Queries.Get.List;
using TMS.Application.CardOrders.Queries.Get.Order;
using TMS.Application.Common.Variables;
using TMS.Contracts.CardOrder;
using TMS.Contracts.CardOrder.Create;
using TMS.Contracts.CardOrder.Get;
using TMS.Contracts.CardOrder.Update;
using TMS.Domain.Cards;
using TMS.Domain.Common.Models;

namespace TMS.Api.Controllers;

[Route("card-order")]
[Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Admin.Role},{Roles.Assistant.AddCardOrder}")]

public class CardOrderController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public CardOrderController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Assistant.AddCardOrder}")]
    [HttpPost]
    public async Task<IActionResult> CreateCardOrder([FromBody] CreateCardOrderRequest request)
    {
        var command = _mapper.Map<CreateCardOrderCommand>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<CardOrderDto>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetCardOrders([FromQuery] GetCardOrdersRequest request)
    {
        var query = _mapper.Map<GetCardOrdersQuery>(request);
        var result = await _mediator.Send(query);
        var response = _mapper.Map<PaginatedList<CardOrderDto>>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCardOrder([FromRoute] string id, [FromBody] UpdateCardOrderRequest request)
    {
        if (id != request.Id)
            return BadRequest("Id in route and body are different");
        var command = _mapper.Map<UpdateCardOrderCommand>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<CardOrderDto>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCardOrder([FromRoute] string id)
    {
        var query = new GetCardOrderQuery(CardOrderId.Create(id));
        var result = await _mediator.Send(query);
        var response = _mapper.Map<CardOrderDetailsDto>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem);
    }
}