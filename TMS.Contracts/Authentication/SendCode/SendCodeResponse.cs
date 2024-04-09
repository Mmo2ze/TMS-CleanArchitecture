namespace TMS.Contracts.Authentication.SendCode;

public record SendCodeResponse(string Token,DateTime ExpireDate);