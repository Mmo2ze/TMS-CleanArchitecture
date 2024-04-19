using ErrorOr;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;

namespace TMS.Infrastructure.Auth;

public class CodeManger : ICodeManger
{
	private static readonly List<VerificationCode> Codes = [];
	private readonly IWhatsappSender _whatsappSender;

	public CodeManger( IWhatsappSender whatsappSender)
	{
		_whatsappSender = whatsappSender;
	}

	public async Task<ErrorOr<DateTime>> GenerateCode(string phone, UserAgent agent)
	{
		var code = new VerificationCode(phone, agent);
		Codes.RemoveAll(x => x.Phone == phone && x.Identify == agent);
		Codes.Add(code);

		var result = await _whatsappSender.SendMessage(phone, code.Code);
		if (result.IsError)
			return result.FirstError;

		return code.ExpireDate;
	}

	public async Task<Error?> VerifyCode(string handel, UserAgent identify, string code)
	{
		await Task.CompletedTask;
		var verificationCode = Codes.FirstOrDefault(x => x.Phone == handel && x.Identify == identify);
		if (verificationCode is null)
			return Errors.Auth.InvalidCode;
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