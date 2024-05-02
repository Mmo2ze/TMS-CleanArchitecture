namespace TMS.Contracts.Authentication.VerifyCode;

public record VerifyCodeResponse(string Token,string? RefreshToken,bool IsRegistered);