using System.Security.Claims;
using ErrorOr;
using MediatR;
using TMS.Application.Authentication.Common;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Variables;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ICookieManger _cookieManger;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IAdminRepository _adminRepository;

    public RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, ICookieManger cookieManger,
        IJwtTokenGenerator jwtTokenGenerator, IAdminRepository adminRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _cookieManger = cookieManger;
        _jwtTokenGenerator = jwtTokenGenerator;
        _adminRepository = adminRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var token = _cookieManger.GetProperty(CookieVariables.RefreshToken);
        if (string.IsNullOrEmpty(token))
            return Errors.Auth.InvalidCredentials;

        var isFound = await _refreshTokenRepository.DeleteRefreshTokenAsync(token);
        if (!isFound)
            return Errors.Auth.InvalidCredentials;

        var phone = _cookieManger.GetProperty(CookieVariables.Phone);
        var agent = Enum.Parse<UserAgent>(_cookieManger.GetProperty(CookieVariables.Agent));
        if (string.IsNullOrEmpty(phone))
            return Errors.Auth.InvalidCredentials;



        var claims = await GetClaims(phone, agent);
        var userId = claims.First(c => c.Type == JwtVariables.CustomClaimTypes.Id).Value;

        return _jwtTokenGenerator.RefreshToken(claims, TimeSpan.FromDays(120), agent, userId);
    }

    private async Task<List<Claim>> GetClaims(string phone, UserAgent agent)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.MobilePhone, phone),
            new(JwtVariables.CustomClaimTypes.Agent, agent.ToString())
        };
        switch (agent)
        {
            case UserAgent.Admin:
                await GetAdminClaims(phone, claims);
                break;
        }

        return claims;
    }

    private async Task GetAdminClaims(string phone, List<Claim> claims)
    {
        var admin = await _adminRepository.GetAdminByPhone(phone);
        claims.Add(new Claim(ClaimTypes.Role, JwtVariables.Roles.AdminR.Role));
        claims.Add(new Claim(JwtVariables.CustomClaimTypes.Id, admin!.Id.Value));
    }
}