namespace TMS.Application.Authentication.Commands.SendCode;

public record SendCodeResult(
	string Token,DateTime ExpireDate);