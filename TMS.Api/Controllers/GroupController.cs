using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Accounts.Commands.Delete;
using TMS.Application.Accounts.Queries.Get;
using TMS.Application.Common.Variables;
using TMS.Application.Groups.Commands.Create;
using TMS.Application.Groups.Commands.Delete;
using TMS.Application.Groups.Commands.Update;
using TMS.Application.Groups.Queries.GetGroups;
using TMS.Application.Sessions.Commands.Delete;
using TMS.Application.Sessions.Queries.Get;
using TMS.Contracts.Account.DTOs;
using TMS.Contracts.Account.Get.List;
using TMS.Contracts.Group.Create;
using TMS.Contracts.Group.GetGroups;
using TMS.Contracts.Group.Update;
using TMS.Contracts.Session.Get;
using TMS.Domain.Account;
using TMS.Domain.Common.Models;
using TMS.Domain.Groups;
using TMS.Domain.Sessions;

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
        var response = _mapper.Map<GetGroupResult>(request);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetGroupsRequest request)
    {
        var command = _mapper.Map<GetGroupsQuery>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<PaginatedList<GetGroupResponse>>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateGroupRequest request)
    {
        var command = _mapper.Map<UpdateGroupCommand>(request);
        command = command with { Id = GroupId.Create(id) };
        var result = await _mediator.Send(command);
        var response = _mapper.Map<UpdateGroupResponse>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var command = new DeleteGroupCommand(GroupId.Create(id));
        var result = await _mediator.Send(command);
        return result.Match(
            _ => Ok("Group deleted successfully"),
            Problem
        );
    }

    [HttpGet("{id}/accounts")]
    public async Task<IActionResult> GetAccounts([FromRoute] string id, [FromQuery] GetAccountsRequest request)
    {
        if(id != request.GroupId)
            return BadRequest("Group id in the route and query string do not match");
        var command = _mapper.Map<GetAccountsQuery>(request);
        
        var result = await _mediator.Send(command);
        var response = _mapper.Map<PaginatedList<AccountSummaryDto>>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem);
    }

    [HttpDelete("{groupId}/accounts/{accountId}")]
    public async Task<IActionResult> DeleteAccount(string groupId, string accountId)
    {
        var command = new DeleteAccountCommand(AccountId.Create(accountId), GroupId.Create(groupId));
        var result = await _mediator.Send(command);
        return result.Match(
            _ => Ok("Account removed successfully"),
            Problem);
    }


    [HttpGet("{id}/sessions")]
    public async Task<IActionResult> GetSessions([FromRoute] string id,[FromQuery] GetSessionsRequest request)
    {
        if(id != request.GroupId)
            return BadRequest("Group id in the route and query string do not match");

        var query = _mapper.Map<GetSessionsQuery>(request);     
        var result = await _mediator.Send(query);
        var response = _mapper.Map<PaginatedList<SessionResponseSummary>>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem);
    }

    [HttpDelete("{groupId}/sessions/{sessionId}")]
    public async Task<IActionResult> DeleteSession(string groupId, string sessionId)
    {
        var command = new DeleteSessionCommand(GroupId.Create(groupId), SessionId.Create(sessionId));
        var result = await _mediator.Send(command);
        return result.Match(
            _ => Ok("session deleted successfully")
            , Problem);
    }
}