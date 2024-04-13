namespace TMS.Application.Authentication.Queries.VerifyCode;

public record VerifyCodeResult(string Token,bool IsRegistered);