using TMS.Domain.Common.Models;

namespace TMS.Application.Authentication.Queries.VerifyCode;

public record VerifyCodeResult(string Token, bool IsRegistered);