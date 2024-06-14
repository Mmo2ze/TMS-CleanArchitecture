using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using TMS.Application.Authentication.Common;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Domain.Common.Errors;

namespace TMS.Application.Authentication.Queries.VerifyCode;

public class VerifyCodeQueryHandler : IRequestHandler<VerifyCodeQuery, ErrorOr<AuthenticationResult>>
{
    private readonly ICodeManger _codeManger;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IClaimsReader _claimsReader;
    private readonly IClaimGenerator _claimGenerator;

    public VerifyCodeQueryHandler(ICodeManger codeManger, IJwtTokenGenerator jwtTokenGenerator,
        IClaimsReader claimsReader,
        IClaimGenerator claimGenerator)
    {
        _codeManger = codeManger;
        _jwtTokenGenerator = jwtTokenGenerator;
        _claimsReader = claimsReader;
        _claimGenerator = claimGenerator;
    }

    [Authorize]
    public async Task<ErrorOr<AuthenticationResult>> Handle(VerifyCodeQuery request, CancellationToken cancellationToken)
    {
        var agentString = _claimsReader.GetByClaimType(CustomClaimTypes.Agent);
        var phone = _claimsReader.GetByClaimType(ClaimTypes.MobilePhone);
        var userId = _claimsReader.GetByClaimType(CustomClaimTypes.Id);
        if (agentString is null || phone is null || userId is null)
            return Errors.Auth.InvalidCredentials;

        var period = TimeSpan.FromDays(30);
        var isRegistered = true;

        var agent = Enum.Parse<UserAgent>(agentString);

        var result = await _codeManger.VerifyCode(phone, agent, request.Code);
        if (result is not null)
            return result.Value;

        var claims = await _claimGenerator.GenerateClaims(userId, agent);
        if (claims.IsError)
            return claims.FirstError;

        var refreshToken = _jwtTokenGenerator.RefreshToken(claims.Value, period, agent, userId);
        if (refreshToken.IsError)
            return refreshToken.FirstError;

        return new AuthenticationResult(refreshToken.Value.Token,  refreshToken.Value.ExpireDate,
            refreshToken.Value.Roles);    }


   
}