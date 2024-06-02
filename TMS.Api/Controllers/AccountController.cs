using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Accounts.Commands.Create;
using TMS.Application.Accounts.Commands.Delete;
using TMS.Application.Accounts.Commands.Update;
using TMS.Application.Accounts.Queries.Get;
using TMS.Contracts.Account.Create;
using TMS.Contracts.Account.DTOs;
using TMS.Contracts.Account.Get.List;
using TMS.Contracts.Account.Update;
using TMS.Domain.Account;
using TMS.Domain.Common.Models;
using TMS.Domain.Groups;

namespace TMS.Api.Controllers;

public class AccountController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public AccountController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        var command = _mapper.Map<CreateAccountCommand>(request);
        var result = await _mediator.Send(command);
        return result.Match(
            _ => Ok(result.Value),
            Problem
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts([FromQuery] GetAccountsRequest request)
    {
        var query = _mapper.Map<GetAccountsQuery>(request);
        var result = await _mediator.Send(query);
        var response = _mapper.Map<PaginatedList<AccountSummaryDto>>(result);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountRequest request, string id)
    {
        var command = _mapper.Map<UpdateAccountCommand>(request);
        command = command with { Id = AccountId.Create(id) };

        var result = await _mediator.Send(command);

        var response = _mapper.Map<AccountSummaryDto>(result.Value);

        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAccountPartial([FromQuery] UpdateAccountPartialRequest request, string id)
    {
        var command = _mapper.Map<UpdateAccountPartialCommand>(request);
        command = command with { Id = AccountId.Create(id) };

        var result = await _mediator.Send(command);

        var response = _mapper.Map<AccountSummaryDto>(result.Value);

        return result.Match(
            _ => Ok(response),
            Problem
        );
    }


    
    
}