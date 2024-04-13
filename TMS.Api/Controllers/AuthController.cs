using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Authentication.Commands;
using TMS.Application.Authentication.Commands.SendCode;
using TMS.Application.Authentication.Queries;
using TMS.Application.Authentication.Queries.VerifyCode;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Variables;
using TMS.Contracts.Authentication.SendCode;
using TMS.Contracts.Authentication.VerifyCode;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Enums;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Parents;
using TMS.Domain.Students;
using TMS.Domain.Teachers;
using TMS.Infrastructure.Auth;

namespace TMS.Api.Controllers;

[ApiController]
public class AuthController : ApiController
{
	private readonly ISender _mediator;
	private readonly IMapper _mapper;
	private ICookieManger _cookieManger;
	private readonly ITeacherRepository _teacherRepository;

	public AuthController(ISender mediator, IMapper mapper, ICookieManger cookieManger, ITeacherRepository teacherRepository)
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
		var response = _mapper.Map<SendCodeResponse>(result.Value);
		return result.Match(
			_ => Ok(response),
			Problem
		);
	}
	[AllowAnonymous]
	[HttpGet("myRoles")]
	public IActionResult MyRoles()
	{
		var token = _cookieManger.GetProperty("Token");
		if (string.IsNullOrEmpty(token))
			return BadRequest("Token not found");
		return Ok(new MyRolesRecord(token));
	}
	[HttpPost("verify-code")]
	public IActionResult VerifyCode([FromBody] VerifyCodeRequest request)
	{
		var command = _mapper.Map<VerifyCodeQuery>(request);
		var result = _mediator.Send(command).Result;
		var response = _mapper.Map<VerifyCodeResponse>(result.Value);
		return result.Match(
			_ => Ok(response),
			Problem
		);
	}

	
}