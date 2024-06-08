using ErrorOr;
using TMS.Domain.Teachers;

namespace TMS.Application.Common.Services;

public interface IWhatsappSender
{
	Task<bool> IsValidNumber(string number);
	Task<ErrorOr<string>> Send(string number, string message,Teacher? teacher = null);
	
	
}