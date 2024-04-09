using TMS.Domain.Assistants;

namespace TMS.Application.Common.Persistence;

public interface IAssistantRepository
{
	Task<bool> IsAssistant(string phone);
	Task<Assistant?> GetAssistantByPhone(string phone);
}