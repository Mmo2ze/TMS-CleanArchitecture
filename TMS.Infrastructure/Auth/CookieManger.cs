using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Services;

namespace TMS.Infrastructure.Auth;

public class CookieManger : ICookieManger
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CookieManger(IHttpContextAccessor contextAccessor, IDateTimeProvider dateTimeProvider)
    {
        _contextAccessor = contextAccessor;
        _dateTimeProvider = dateTimeProvider;
    }

    public void SetProperty(string key, object value, TimeSpan period)
    {
        SetProperty(key, value, _dateTimeProvider.Now.Add(period));
    }

    public void SetProperty(string key, object value, DateTime date)
    {
        var options = new CookieOptions
        {
            Path = "/",
            Expires = date,
            // Accepts only secure cookies
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true
        };
        var stringValue = value is not string ? JsonSerializer.Serialize(value) : (string)value;
        _contextAccessor.HttpContext?.Response.Cookies.Append(key, stringValue, options);
    }

    [Authorize]
    public string? GetPropertyByClaimType(string key)
    {
        return _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == key)?.Value;
    }

    public string GetProperty(string key)
    {
        return _contextAccessor.HttpContext!.Request.Cookies[key]!;
        
    }
}