using System.Security.Claims;
using ErrorOr;
using MediatR;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Variables;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Authentication.Commands.SendCode;

public class SendCodeCommandHandler : IRequestHandler<SendCodeCommand, ErrorOr<SendCodeResult>>
{
	private readonly ICodeManger _codeManger;
	private readonly IJwtTokenGenerator _tokenGenerator;
	private readonly IAdminRepository _adminRepository;
	private readonly ITeacherRepository _teacherRepository;
	private readonly IAssistantRepository _assistantRepository;
	

	public SendCodeCommandHandler(ICodeManger codeManger,
		IJwtTokenGenerator tokenGenerator,
		IAdminRepository adminRepository,
		ITeacherRepository teacherRepository,
		IAssistantRepository assistantRepository)
	{
		_codeManger = codeManger;
		_tokenGenerator = tokenGenerator;
		_adminRepository = adminRepository;
		_teacherRepository = teacherRepository;
		_assistantRepository = assistantRepository;
	}

	public async Task<ErrorOr<SendCodeResult>> Handle(SendCodeCommand request, CancellationToken cancellationToken)
	{
		switch(request.UserAgent)
		{
			case UserAgent.Admin:
				if (!await _adminRepository.IsAdmin(request.Phone))
					return Errors.Auth.YouAreNotAdmin;
				break;
			case UserAgent.Teacher:
				if (!await _teacherRepository.IsTeacher(request.Phone, new CancellationToken()) || !await _assistantRepository.IsAssistant(request.Phone))
					return Errors.Auth.NotTeacherOrAssistant;
				break;
		}
		var expireDate = await _codeManger.GenerateCode(request.Phone, request.UserAgent);
		if (expireDate.IsError)
			return expireDate.FirstError;
		var token = GenerateToken(request, expireDate);

		return new SendCodeResult(token, expireDate.Value);
	}

	private string GenerateToken(SendCodeCommand request, ErrorOr<DateTime> expireDate)
	{
	
		var claims = new List<Claim>
		{
			new(ClaimTypes.MobilePhone, request.Phone),
			new(ClaimTypes.Role, JwtVariables.Roles.CodeSent),
			new(JwtVariables.CustomClaimTypes.Agent, request.UserAgent.ToString())
		};
		return _tokenGenerator.GenerateToken(claims, expireDate.Value);
	}
}