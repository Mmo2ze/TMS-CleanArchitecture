using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Common.Variables;
using TMS.Application.Groups.Commands.Create;
using TMS.Application.Groups.Queries.GetGroups;
using TMS.Contracts.Group.Create;
using TMS.Contracts.Group.GetGroups;
using TMS.Domain.Common.Models;

namespace TMS.Api.Controllers;

[Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Teacher.Assistant}")]
public class GroupController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GroupController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupRequest request)
    {
        var command = _mapper.Map<CreateGroupCommand>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<CreateGroupRequest>(request);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetGroupsRequest request)
    {
        var command = _mapper.Map<GetGroupsCommand>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<PaginatedList<GetGroupResponse>>(result.Value);
        return result.Match(
            _ =>Ok(response),
            Problem
        );
    }
}