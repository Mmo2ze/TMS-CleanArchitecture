using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using TMS.Application.Common.Interfaces.Auth;

namespace TMS.Infrastructure.Auth;

public class CookieManger : ICookieManger
{
	private readonly IHttpContextAccessor _contextAccessor;

	public CookieManger(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;
	}

	public void SetProperty(string key, object value,TimeSpan period)
	{
		CookieOptions options = new CookieOptions();
		options.Path = "/";
		options.Expires = DateTime.Now.Add(period);
		// Accepts only secure cookies
		options.HttpOnly = true;
		options.SameSite = SameSiteMode.None;
		options.Secure = true;
		var stringValue = value is not string ?JsonSerializer.Serialize(value):(string)value;
		_contextAccessor.HttpContext.Response.Cookies.Append("Token",stringValue, options);
	}
	[Authorize]
	public string? GetPropertyByClaimType(string key)
	{
		return _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == key)?.Value;
	}

	public string GetProperty(string key)
	{
		return _contextAccessor.HttpContext.Request.Cookies[key];
	}
}