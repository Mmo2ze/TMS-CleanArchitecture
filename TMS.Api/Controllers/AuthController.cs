using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Authentication.Commands.RefreshToken;
using TMS.Application.Authentication.Commands.SendCode;
using TMS.Application.Authentication.Queries.VerifyCode;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Contracts.Authentication.SendCode;
using TMS.Contracts.Authentication.VerifyCode;
using TMS.Domain.Common.Repositories;

namespace TMS.Api.Controllers;

[ApiController]
public class AuthController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly ICookieManger _cookieManger;
    private readonly ITeacherRepository _teacherRepository;

    public AuthController(ISender mediator, IMapper mapper, ICookieManger cookieManger,
        ITeacherRepository teacherRepository)
    {
        _mediator = mediator;
        _mapper = mapper;
        _cookieManger = cookieManger;
        _teacherRepository = teacherRepository;
    }

    [AllowAnonymous]
    [HttpPost("send-code")]
    public IActionResult SendCode([FromBody] SendCodeRequest request)
    {
        var command = _mapper.Map<SendCodeCommand>(request);
        var result = _mediator.Send(command).Result;
        var response = _mapper.Map<AuthenticationResponse>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }



    [HttpPost("verify-code")]
    public IActionResult VerifyCode([FromBody] VerifyCodeRequest request)
    {
        var command = _mapper.Map<VerifyCodeQuery>(request);
        var result = _mediator.Send(command).Result;
        var response = _mapper.Map<AuthenticationResponse>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public IActionResult RefreshToken()
    {
        var myConfigVar = Environment.GetEnvironmentVariable("test");
        Console.WriteLine("fuck",myConfigVar);
        var command = new RefreshTokenCommand();
        var result = _mediator.Send(command).Result;
        return result.Match(
            _ => Ok(result.Value),
            Problem
        );
    }
}