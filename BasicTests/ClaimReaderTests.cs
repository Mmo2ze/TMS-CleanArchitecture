using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using TMS.Application.Common.Services;
using TMS.Infrastructure.Services;

namespace BasicTests;

public class ClaimReaderTests
{
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly IClaimsReader _claimsReader;

    public ClaimReaderTests()
    {
        _claimsReader = new ClaimsReader(_httpContextAccessor);
    }

    [Fact]
    public void GetByClaimTypeShouldReturnClaimValue()
    {
        var claimType = "claimType";
        var claimValue = "claimValue";
        _httpContextAccessor.HttpContext.User.Claims.Returns(new List<Claim>
        {
            new Claim(claimType, claimValue)
        });
        var result = _claimsReader.GetByClaimType(claimType);
        Assert.Equal(claimValue, result);
    }

    [Fact]
    public void GetByClaimTypeShouldReturnNullWhenClaimNotFound()
    {
        var claimType = "claimType";
        _httpContextAccessor.HttpContext.User.Claims.Returns([]);
        var result = _claimsReader.GetByClaimType(claimType);
        Assert.Null(result);
    }

    [Fact]
    public void GetRolesShouldReturnRoles()
    {
        var roles = new List<string> { "role1", "role2" };
        _httpContextAccessor.HttpContext.User.Claims.Returns(
            roles.Select(r =>
                    new Claim(ClaimTypes.Role, r))
                .ToList());
        
        var result = _claimsReader.GetRoles();
        Assert.Equal(roles, result);
    }
    
    
}