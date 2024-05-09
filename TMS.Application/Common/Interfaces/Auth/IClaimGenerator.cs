using System.Security.Claims;
using ErrorOr;
using TMS.Application.Common.Enums;

namespace TMS.Application.Common.Interfaces.Auth;

public interface IClaimGenerator
{
    Task<ErrorOr<List<Claim>>> GenerateClaims(string userId, UserAgent agent);
}