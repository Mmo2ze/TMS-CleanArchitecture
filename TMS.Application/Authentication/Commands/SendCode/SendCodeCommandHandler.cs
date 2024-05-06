using System.Security.Claims;
using ErrorOr;
using MassTransit.Initializers;
using MediatR;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Services;
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
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICookieManger _cookieManger;

    public SendCodeCommandHandler(ICodeManger codeManger,
        IJwtTokenGenerator tokenGenerator,
        IAdminRepository adminRepository,
        ITeacherRepository teacherRepository,
        IAssistantRepository assistantRepository, ICookieManger cookieManger, IDateTimeProvider dateTimeProvider)
    {
        _codeManger = codeManger;
        _tokenGenerator = tokenGenerator;
        _adminRepository = adminRepository;
        _teacherRepository = teacherRepository;
        _assistantRepository = assistantRepository;
        _cookieManger = cookieManger;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<SendCodeResult>> Handle(SendCodeCommand request, CancellationToken cancellationToken)
    {
        string? userId = null;
        switch (request.UserAgent)
        {
            case UserAgent.Admin:
                var admin = await _adminRepository.GetAdminByPhone(request.Phone);
                if (admin is null)
                    return Errors.Auth.UnauthorizedToBeAdmin;
                userId = admin.Id.Value;
                break;
            case UserAgent.Teacher:
                userId = _teacherRepository.GetByPhone(request.Phone, cancellationToken).Select(a => a?.Id).Result
                             ?.Value
                         ?? _assistantRepository.GetAssistantByPhone(request.Phone).Select(a => a?.Id).Result?.Value;
                if (userId is null)
                    return Errors.Auth.NotTeacherOrAssistant;
                break;
        }

        var expireDate = await _codeManger.GenerateCode(request.Phone, request.UserAgent);
        if (expireDate.IsError)
            return expireDate.FirstError;
        var result = GenerateToken(request, expireDate.Value, request.UserAgent, userId);
        return result;
    }

    private ErrorOr<SendCodeResult> GenerateToken(SendCodeCommand request, DateTime expireDate,
        UserAgent agent, string? userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.MobilePhone, request.Phone),
            new Claim(CustomClaimTypes.Id, userId!),
            new(ClaimTypes.Role, Roles.CodeSent),
            new(CustomClaimTypes.Agent, request.UserAgent.ToString())
        };
        var refreshToken = _tokenGenerator.RefreshToken(claims, expireDate - _dateTimeProvider.Now, agent, userId);
        if (refreshToken.IsError)
            return refreshToken.FirstError;
        return new SendCodeResult(refreshToken.Value.Token, expireDate);
    }
}