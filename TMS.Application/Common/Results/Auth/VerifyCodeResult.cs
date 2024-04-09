namespace TMS.Application.Common.Results.Auth;

public record VerifyCodeResult(string Token,bool IsRegistered);