using ErrorOr;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Domain.Common.Errors;

namespace TMS.Infrastructure.Auth;

public class CodeManger : ICodeManger
{
    private static readonly List<VerificationCode> Codes = [];
    

    public VerificationCode GenerateCode(string phone, UserAgent agent)
    {
        var code = new VerificationCode(phone, agent);

        Codes.RemoveAll(x => x.Phone == phone && x.Identify == agent);

        Codes.Add(code);

        return code;
    }

    public async Task<Error?> VerifyCode(string handel, UserAgent identify, string code)
    {
        await Task.CompletedTask;
        var verificationCode = Codes.FirstOrDefault(x => x.Phone == handel && x.Identify == identify);
        if (verificationCode is null)
            return Errors.Auth.InvalidCredentials;
        verificationCode.TryCount++;
        if (verificationCode.ExpireDate < DateTime.Now)
            return Errors.Auth.CodeExpired(verificationCode.ExpireDate);
        if (verificationCode.Code != code || verificationCode.TryCount > 5)
        {
            return verificationCode.TryCount > 5 ? Errors.Auth.ToManyTry : Errors.Auth.InvalidCode;
        }

        Codes.Remove(verificationCode);
        return null;
    }
}