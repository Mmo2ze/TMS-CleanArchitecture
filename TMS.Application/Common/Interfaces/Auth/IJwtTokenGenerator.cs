
using System.Security.Claims;
using TMS.Application.Common.Enums;

namespace TMS.Application.Common.Interfaces.Auth;

public interface IJwtTokenGenerator
{
	string GenerateToken(List<Claim> claims,DateTime expireTime );
	string GenerateToken(List<Claim> claims, TimeSpan period);
}