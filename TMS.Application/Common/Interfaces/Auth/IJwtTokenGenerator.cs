
using System.Security.Claims;

namespace TMS.Application.Common.Interfaces.Auth;

public interface IJwtTokenGenerator
{
	string GenerateToken(List<Claim> claims,DateTime expireTime );
	string GenerateToken(List<Claim> claims, TimeSpan period);
}