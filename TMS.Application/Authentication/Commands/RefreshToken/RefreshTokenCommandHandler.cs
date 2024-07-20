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
    private readonly IClaimGenerator _claimGenerator;


    public RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, ICookieManger cookieManger,
        IJwtTokenGenerator jwtTokenGenerator, IClaimGenerator claimGenerator)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _cookieManger = cookieManger;
        _jwtTokenGenerator = jwtTokenGenerator;
        _claimGenerator = claimGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var token = _cookieManger.GetProperty(CookieVariables.RefreshToken);
        if (string.IsNullOrEmpty(token))
            return Errors.Auth.InvalidCredentials;

        var refreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(token, cancellationToken);
        if (refreshToken is null)
            return Errors.Auth.InvalidCredentials;

        var userId = _cookieManger.GetProperty(CookieVariables.Id);
        var agent = Enum.Parse<UserAgent>(_cookieManger.GetProperty(CookieVariables.Agent));
        if (string.IsNullOrEmpty(userId))
            return Errors.Auth.InvalidCredentials;


        var claims = await _claimGenerator.GenerateClaims(userId, agent);

        return
            claims.IsError
                ? claims.FirstError
                : await _jwtTokenGenerator.RefreshToken(claims.Value, TimeSpan.FromDays(120), agent, userId,refreshToken, cancellationToken);
    }
}