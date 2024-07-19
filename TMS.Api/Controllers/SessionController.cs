using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Common.Variables;
using TMS.Application.Sessions.Commands.Create;
using TMS.Application.Sessions.Queries.Get;
using TMS.Contracts.Session.Create;
using TMS.Contracts.Session.Get;
using TMS.Domain.Common.Models;

namespace TMS.Api.Controllers;

[Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Assistant.Role}")]
public class SessionController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public SessionController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }


    [Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Assistant.AddGroup}")]
    [HttpPost]
    public IActionResult Create([FromBody] CreateSessionRequest request)
    {
        var command = _mapper.Map<CreateSessionCommand>(request);
        var result = _mediator.Send(command).Result;
        var response = _mapper.Map<SessionResponseSummary>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
    
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetSessionsRequest request)
    {
        var query = _mapper.Map<GetSessionsQuery>(request);
        var result = await _mediator.Send(query);
        var response = _mapper.Map<PaginatedList<SessionResponseSummary>>(result.Value);
        return Ok(response);
    }
}