using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using TMS.Application.Common.Services;

namespace TMS.Infrastructure.Services;

[Authorize]
public class ClaimsReader : IClaimsReader
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimsReader(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetByClaimType(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    }

    public List<string> GetRoles()
    {
        return _httpContextAccessor.HttpContext?.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)
            .ToList() ?? [];
    }
}