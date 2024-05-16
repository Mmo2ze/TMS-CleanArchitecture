using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Accounts.Commands.Create;
using TMS.Contracts.Account.Create;

namespace TMS.Api.Controllers;

public class AccountController:ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public AccountController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost()]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        var command = _mapper.Map<CreateAccountCommand>(request);
        var result = await _mediator.Send(command);
        return result.Match(
            _ => Ok(result.Value),
            Problem
        );
    }
}