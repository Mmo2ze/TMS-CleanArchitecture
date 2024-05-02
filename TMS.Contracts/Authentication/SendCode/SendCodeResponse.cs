namespace TMS.Contracts.Authentication.SendCode;

public record SendCodeResponse(string Token,string? RefreshToken,DateTime ExpireDate);