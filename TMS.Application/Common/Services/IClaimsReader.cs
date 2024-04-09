namespace TMS.Application.Common.Services;

public interface IClaimsReader
{
	string? GetByClaimType(string claimType);
}