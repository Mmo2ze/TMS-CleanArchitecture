namespace TMS.Application.Common.Results.Auth;

public record SendCodeResult(
	string Token,DateTime ExpireDate);