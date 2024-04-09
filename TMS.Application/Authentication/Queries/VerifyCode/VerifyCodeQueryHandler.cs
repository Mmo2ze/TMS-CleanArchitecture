using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Persistence;
using TMS.Application.Common.Results.Auth;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Domain.Common.Errors;

namespace TMS.Application.Authentication.Queries.VerifyCode;

public class VerifyCodeQueryHandler : IRequestHandler<VerifyCodeQuery, ErrorOr<VerifyCodeResult>>
{
	private readonly ICodeManger _codeManger;
	private readonly IJwtTokenGenerator _jwtTokenGenerator;
	private readonly IClaimsReader _claimsReader;
	private readonly IAdminRepository _adminRepository;
	private readonly ITeacherRepository _teacherRepository;
	private readonly IAssistantRepository _assistantRepository;
	private readonly IParentRepository _parentRepository;
	private readonly IStudentRepository _studentRepository;

	public VerifyCodeQueryHandler(ICodeManger codeManger, IJwtTokenGenerator jwtTokenGenerator,
		IClaimsReader claimsReader, IAdminRepository adminRepository, IStudentRepository studentRepository,
		ITeacherRepository teacherRepository, IAssistantRepository assistantRepository,
		IParentRepository parentRepository)
	{
		_codeManger = codeManger;
		_jwtTokenGenerator = jwtTokenGenerator;
		_claimsReader = claimsReader;
		_adminRepository = adminRepository;
		_studentRepository = studentRepository;
		_teacherRepository = teacherRepository;
		_assistantRepository = assistantRepository;
		_parentRepository = parentRepository;
	}

	[Authorize]
	public async Task<ErrorOr<VerifyCodeResult>> Handle(VerifyCodeQuery request, CancellationToken cancellationToken)
	{
		var agentString = _claimsReader.GetByClaimType(JwtVariables.CustomClaimTypes.Agent);
		var phone = _claimsReader.GetByClaimType(ClaimTypes.MobilePhone);
		if (agentString is null || phone is null)
			return Errors.Auth.InvalidCredentials;

		var period = TimeSpan.FromDays(30);
		var isRegistered = true;

		var agent = Enum.Parse<UserAgent>(agentString);

		var result = await _codeManger.VerifyCode(phone, agent, request.Code);
		if (result is not null)
			return result.Value;

		var claims = new List<Claim>
		{
			new(ClaimTypes.MobilePhone, phone),
			new(JwtVariables.CustomClaimTypes.Agent, agentString)
		};

		switch (agent)
		{
			case UserAgent.Admin:
				await GetAdminClaims(phone, claims);
				break;

			case UserAgent.Teacher:
				await GetTeacherClaims(phone, claims);

				break;
			case UserAgent.Student:
				(period, isRegistered) = await GetStudentClaims(phone, claims);

				break;
			case UserAgent.Parent:
				(period, isRegistered) = await GetParentClaims(phone, claims);
				break;
			default:
				return Errors.Auth.InvalidCredentials;
		}

		var token = _jwtTokenGenerator.GenerateToken(claims, period);
		var response = new VerifyCodeResult(token, isRegistered);
		return response;
	}


	private async Task<(TimeSpan, bool)> GetParentClaims(string phone, List<Claim> claims)
	{
		var period = TimeSpan.FromDays(90);
		var isRegistered = true;
		var parent = await _parentRepository.GetParentByPhone(phone);
		if (parent is not null)
		{
			claims.Add(new Claim(ClaimTypes.Role, JwtVariables.Roles.ParentR.Role));
			claims.Add(new Claim(JwtVariables.CustomClaimTypes.Id, parent.Id.ToString()));
			claims.Add(new Claim(ClaimTypes.Name, parent.Name));
		}
		else
		{
			claims.Add(new Claim(ClaimTypes.Role, JwtVariables.Roles.ParentR.ParentNonRegister));
			isRegistered = false;
			period = TimeSpan.FromHours(1);
		}

		return (period, isRegistered);
	}

	private async Task<(TimeSpan, bool)> GetStudentClaims(string phone, List<Claim> claims)
	{
		var period = TimeSpan.FromDays(90);
		var isRegistered = true;
		var student = await _studentRepository.GetStudentByPhone(phone);
		if (student is not null)
		{
			claims.Add(new Claim(ClaimTypes.Role, JwtVariables.Roles.StudentR.Role));
			claims.Add(new Claim(JwtVariables.CustomClaimTypes.Id, student.Id.ToString()));
			claims.Add(new Claim(ClaimTypes.Name, student.Name));
		}
		else
		{
			claims.Add(new Claim(ClaimTypes.Role, JwtVariables.Roles.StudentR.StudentNonRegister));
			isRegistered = false;
			period = TimeSpan.FromHours(1);
		}

		return (period, isRegistered);
	}

	private async Task GetTeacherClaims(string phone, List<Claim> claims)
	{
		var teacher = await _teacherRepository.GetTeacherByPhone(phone);
		if (teacher is not null)
		{
			claims.Add(new Claim(ClaimTypes.Role, JwtVariables.Roles.TeacherR.Role));
			claims.Add(new Claim(JwtVariables.CustomClaimTypes.Id, teacher.Id.ToString()));
			claims.Add(new Claim(ClaimTypes.Name, teacher.Name));
		}
		else
		{
			var assistant = await _assistantRepository.GetAssistantByPhone(phone);
			claims.Add(new Claim(ClaimTypes.Role, JwtVariables.Roles.TeacherR.Assistant));
			claims.Add(new Claim(JwtVariables.CustomClaimTypes.Id, assistant!.Id.ToString()));
			claims.Add(new Claim(ClaimTypes.Name, assistant.Name));
		}
	}

	private async Task GetAdminClaims(string phone, List<Claim> claims)
	{
		var admin = await _adminRepository.GetAdminByPhone(phone);
		claims.Add(new Claim(ClaimTypes.Role, JwtVariables.Roles.AdminR.Role));
		claims.Add(new Claim(JwtVariables.CustomClaimTypes.Id, admin!.Id.ToString()));
	}
}