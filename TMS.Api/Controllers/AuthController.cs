using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Authentication.Commands;
using TMS.Application.Authentication.Queries;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Persistence;
using TMS.Application.Common.Variables;
using TMS.Contracts.Authentication.SendCode;
using TMS.Contracts.Authentication.VerifyCode;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Enums;
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
	
	[AllowAnonymous]
	[HttpPost]
	public IActionResult AddTeacher([FromBody] string teacherName)
	{
	var teacher = Teacher.Create(teacherName,teacherName,Subject.Maths,DateTime.UtcNow.AddYears(1));
	var assistant = Assistant.Create(teacherName,teacherName,teacherName,teacher.Id);
	var student = Student.Create(teacherName,Gender.Male,null,teacher.Id.Value);
	var payment = Payment.Create(student.Id,100,DateTime.UtcNow,DateOnly.FromDateTime(DateTime.UtcNow),teacher.Id,assistant.Id);
	var parent = Parent.Create(teacherName,Gender.Female,teacherName,student.Phone);
	_teacherRepository.Add(teacher);
	return Ok(teacher);
	}
	
}