using ErrorOr;
using TMS.Application.Common.Enums;

namespace TMS.Application.Common.Interfaces.Auth;

public interface ICodeManger
{
	Task<ErrorOr<DateTime>> GenerateCode(string phone,  UserAgent agent);
	Task<Error?> VerifyCode(string phone, UserAgent identify, string code);
}

public record VerificationCode
{
	private DateTime _expireDate = DateTime.Now.AddMinutes(10);
	public VerificationCode(string phone, UserAgent identify )
	{
		Phone = phone;
		Identify = identify;
		Code = RandomCode();
		ExpireDate = DateTime.Now.AddMinutes(10);
	}

	public string Phone { get; set; }
	public UserAgent Identify { get; set; }
	public string Code { get; set; } = null!;
	public DateTime ExpireDate { get; set; }
	public int TryCount { get; set; }
	private static string RandomCode()
	{
		var random = new Random();
		return random.Next(100000, 999999).ToString();

	}
}