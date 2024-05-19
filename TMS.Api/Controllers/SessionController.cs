using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Common.Variables;
using TMS.Application.Sessions;
using TMS.Contracts.Session.Create;

namespace TMS.Api.Controllers;

[Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Teacher.Assistant}")]
public class SessionController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public SessionController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }


    [HttpPost]
    public IActionResult Create([FromBody] CreateSessionRequest request)
    {
        var command = _mapper.Map<CreateSessionCommand>(request);
        var result = _mediator.Send(command).Result;
        return result.Match(
            _ => Ok(result.Value),
            Problem
        );
    }
}