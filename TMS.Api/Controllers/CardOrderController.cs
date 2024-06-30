using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TMS.Application.CardOrders.Commands.Create;
using TMS.Application.CardOrders.Queries.Get;
using TMS.Contracts.CardOrder;
using TMS.Contracts.CardOrder.Get;
using TMS.Domain.Common.Models;

namespace TMS.Api.Controllers;

[Route("card-order")]
public class CardOrderController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public CardOrderController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

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

    [HttpGet()]
    public async Task<IActionResult> GetCardOrders([FromQuery] GetCardOrdersRequest request)
    {
        var query = _mapper.Map<GetCardOrdersQuery>(request);
        var result = await _mediator.Send(query);
        var response = _mapper.Map<PaginatedList<CardOrderDto>>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem);
    }
}