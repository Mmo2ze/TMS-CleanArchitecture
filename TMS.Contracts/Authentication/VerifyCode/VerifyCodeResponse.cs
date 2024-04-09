namespace TMS.Contracts.Authentication.VerifyCode;

public record VerifyCodeResponse(string Token,bool IsRegistered);