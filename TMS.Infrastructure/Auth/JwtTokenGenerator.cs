using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Services;

namespace TMS.Infrastructure.Auth;

public class JwtTokenGenerator : IJwtTokenGenerator
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly JwtSettings _jwtSettings;
	private readonly ICookieManger _cookieManger;

	public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions, ICookieManger cookieManger)
	{
		_dateTimeProvider = dateTimeProvider;
		_cookieManger = cookieManger;
		_jwtSettings = jwtOptions.Value;
	}

	public string GenerateToken(List<Claim> claims,DateTime expireTime )
	{
		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
			SecurityAlgorithms.HmacSha256);

		var securityToken = new JwtSecurityToken(
			issuer: _jwtSettings.Issuer,
			claims: claims,
			audience: _jwtSettings.Audience,
			expires: expireTime,
			signingCredentials: signingCredentials);

		var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
		_cookieManger.SetProperty("Token", token, expireTime - _dateTimeProvider.Now);
		return token;
	}
	public string GenerateToken(List<Claim> claims, TimeSpan period)
	{
		return GenerateToken(claims, _dateTimeProvider.Now.Add(period));
	}
	
}