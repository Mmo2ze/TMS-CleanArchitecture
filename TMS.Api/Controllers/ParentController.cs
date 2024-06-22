using MapsterMapper;
using MassTransit.RabbitMqTransport;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Parents.Commands.Create;
using TMS.Application.Parents.Queries.Get;
using TMS.Contracts.Parent.Create;
using TMS.Contracts.Parent.Get;
using TMS.Domain.Common.Models;

namespace TMS.Api.Controllers;

public class ParentController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ParentController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateParentRequest request)
    {
        var command = _mapper.Map<CreateParentCommand>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<ParentDto>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetParentsRequest request)
    {
        var query = _mapper.Map<GetParentsQuery>(request);
        var result = await _mediator.Send(query);
        return result.Match(
            _ => Ok(_mapper.Map<PaginatedList<ParentDto>>(result.Value)),
            Problem
        );
    }
}