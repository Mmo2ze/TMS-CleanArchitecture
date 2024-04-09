using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Common.Variables;
using TMS.Domain.Admins;

namespace TMS.Api.Controllers;

[Authorize(Roles=JwtVariables.Roles.AdminR.Role)]
public class TeacherController : ApiController
{
	
	[HttpGet]
	public IActionResult Get()
	{
		return Ok("Teacher");
	}
	
	[AllowAnonymous]
	[HttpGet("adminId")]
	public IActionResult GetAdminId()
	{
		
		return Ok(AdminId.CreateUnique());
	}
}