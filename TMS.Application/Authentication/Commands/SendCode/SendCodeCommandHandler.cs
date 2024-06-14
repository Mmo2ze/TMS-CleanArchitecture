using System.Security.Claims;
using ErrorOr;
using MassTransit;
using MassTransit.Initializers;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using TMS.Application.Authentication.Common;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Domain.Common.Enums;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.MessagingContracts.Authentication;

namespace TMS.Application.Authentication.Commands.SendCode;

public class SendCodeCommandHandler : IRequestHandler<SendCodeCommand, ErrorOr<AuthenticationResult>>
{
    private readonly ICodeManger _codeManger;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IAdminRepository _adminRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IAssistantRepository _assistantRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public SendCodeCommandHandler(ICodeManger codeManger,
        IJwtTokenGenerator tokenGenerator,
        IAdminRepository adminRepository,
        ITeacherRepository teacherRepository,
        IAssistantRepository assistantRepository,
        IPublishEndpoint publishEndpoint)
    {
        _codeManger = codeManger;
        _tokenGenerator = tokenGenerator;
        _adminRepository = adminRepository;
        _teacherRepository = teacherRepository;
        _assistantRepository = assistantRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(SendCodeCommand request,
        CancellationToken cancellationToken)
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

        var verificationCode = _codeManger.GenerateCode(request.Phone, request.UserAgent);
        await _publishEndpoint.Publish(new VerificationCodeCreatedEvent(verificationCode.Code, verificationCode.Phone),
            cancellationToken);
        var result = GenerateToken(request, verificationCode.ExpireDate, request.UserAgent, userId);

        return result;
    }

    private ErrorOr<AuthenticationResult> GenerateToken(SendCodeCommand request, DateTime expireDate,
        UserAgent agent, string? userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.MobilePhone, request.Phone),
            new Claim(CustomClaimTypes.Id, userId!),
            new(ClaimTypes.Role, Roles.CodeSent),
            new(CustomClaimTypes.Agent, request.UserAgent.ToString())
        };
        var refreshToken = _tokenGenerator.RefreshToken(claims, expireDate - DateTime.UtcNow, agent, userId);
        if (refreshToken.IsError)
            return refreshToken.FirstError;
        return refreshToken.Value;
    }
}