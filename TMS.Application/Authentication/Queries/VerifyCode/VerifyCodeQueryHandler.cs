using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

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
	private readonly ICookieManger _cookieManger;

	public VerifyCodeQueryHandler(ICodeManger codeManger, IJwtTokenGenerator jwtTokenGenerator,
		IClaimsReader claimsReader, IAdminRepository adminRepository, IStudentRepository studentRepository,
		ITeacherRepository teacherRepository, IAssistantRepository assistantRepository,
		IParentRepository parentRepository, ICookieManger cookieManger)
	{
		_codeManger = codeManger;
		_jwtTokenGenerator = jwtTokenGenerator;
		_claimsReader = claimsReader;
		_adminRepository = adminRepository;
		_studentRepository = studentRepository;
		_teacherRepository = teacherRepository;
		_assistantRepository = assistantRepository;
		_parentRepository = parentRepository;
		_cookieManger = cookieManger;
	}

	[Authorize]
	public async Task<ErrorOr<VerifyCodeResult>> Handle(VerifyCodeQuery request, CancellationToken cancellationToken)
	{
		var agentString = _claimsReader.GetByClaimType(CustomClaimTypes.Agent);
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
			new(CustomClaimTypes.Agent, agentString)
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
		var userId = _claimsReader.GetByClaimType(CustomClaimTypes.Id);
		var refreshToken = _jwtTokenGenerator.RefreshToken(claims, period,agent,userId);
		if (refreshToken.IsError)
			return refreshToken.FirstError;
		
		return new VerifyCodeResult(refreshToken.Value.Token, isRegistered);
		
	}


	private async Task<(TimeSpan, bool)> GetParentClaims(string phone, List<Claim> claims)
	{
		var period = TimeSpan.FromDays(90);
		var isRegistered = true;
		var parent = await _parentRepository.GetParentByPhone(phone);
		if (parent is not null)
		{
			claims.Add(new Claim(ClaimTypes.Role, Roles.Parent.Role));
			claims.Add(new Claim(CustomClaimTypes.Id, parent.Id.ToString()));
			claims.Add(new Claim(ClaimTypes.Name, parent.Name));
		}
		else
		{
			claims.Add(new Claim(ClaimTypes.Role, Roles.Parent.ParentNonRegister));
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
			claims.Add(new Claim(ClaimTypes.Role, Roles.Student.Role));
			claims.Add(new Claim(CustomClaimTypes.Id, student.Id.ToString()));
			claims.Add(new Claim(ClaimTypes.Name, student.Name));
		}
		else
		{
			claims.Add(new Claim(ClaimTypes.Role, Roles.Student.StudentNonRegister));
			isRegistered = false;
			period = TimeSpan.FromHours(1);
		}

		return (period, isRegistered);
	}

	private async Task GetTeacherClaims(string phone, List<Claim> claims)
	{
		var teacher = await _teacherRepository.GetByPhone(phone);
		if (teacher is not null)
		{
			claims.Add(new Claim(ClaimTypes.Role, Roles.Teacher.Role));
			claims.Add(new Claim(CustomClaimTypes.Id, teacher.Id.Value));
			claims.Add(new Claim(ClaimTypes.Name, teacher.Name));
			claims.Add(new Claim(CustomClaimTypes.TeacherId , teacher.Id.Value));

		}
		else
		{
			var assistant = await _assistantRepository.GetAssistantByPhone(phone);
			claims.Add(new Claim(ClaimTypes.Role, Roles.Teacher.Assistant));
			claims.Add(new Claim(CustomClaimTypes.Id, assistant!.Id.Value));
			claims.Add(new Claim(ClaimTypes.Name, assistant.Name));
			claims.Add(new Claim(CustomClaimTypes.TeacherId , assistant.TeacherId.Value));
		}
	}

	private async Task GetAdminClaims(string phone, List<Claim> claims)
	{
		var admin = await _adminRepository.GetAdminByPhone(phone);
		claims.Add(new Claim(ClaimTypes.Role, Roles.Admin.Role));
		claims.Add(new Claim(CustomClaimTypes.Id, admin!.Id.Value));
	}
}