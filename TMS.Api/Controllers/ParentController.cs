using MapsterMapper;
using MassTransit.RabbitMqTransport;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Parents.Commands.Create;
using TMS.Contracts.Parent.Create;

namespace TMS.Api.Controllers;

public class ParentController:ApiController
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
        var command = _mapper.Map<CreateParentComamnd>(request);
        var result = await _mediator.Send(command);
        return result.Match(
            _ => Ok(result.Value.Value),
            Problem
        );
    }
}