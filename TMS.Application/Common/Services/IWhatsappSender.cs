using ErrorOr;

namespace TMS.Application.Common.Services;

public interface IWhatsappSender
{
	Task<bool> IsValidNumber(string number);
	Task<ErrorOr<string>> Send(string number, string message);
	
	
}