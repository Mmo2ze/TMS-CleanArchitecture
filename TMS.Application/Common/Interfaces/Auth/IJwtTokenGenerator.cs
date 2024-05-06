
using System.Security.Claims;
using ErrorOr;
using TMS.Application.Authentication.Common;
using TMS.Application.Common.Enums;

namespace TMS.Application.Common.Interfaces.Auth;

public interface IJwtTokenGenerator
{
	string GenerateJwtToken(List<Claim> claims, DateTime expireTime, UserAgent agent, string? userId);
	string GenerateJwtToken(List<Claim> claims, TimeSpan period,UserAgent agent,string? userId);
	ErrorOr<AuthenticationResult> RefreshToken(List<Claim> claims, TimeSpan expireTime , UserAgent agent, string userId);
}