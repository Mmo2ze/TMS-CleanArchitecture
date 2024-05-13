using Microsoft.EntityFrameworkCore;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Persistence.Repositories;

public class AssistantRepository:Repository<Assistant,AssistantId>,IAssistantRepository
{


	public Task<bool> IsAssistant(string phone)
	{
		return DbContext.Assistants.AnyAsync(a => a.Phone == phone);
	}

	public Task<Assistant?> GetAssistantByPhone(string phone)
	{
		return DbContext.Assistants.FirstOrDefaultAsync(a => a.Phone == phone);
	}

	public AssistantRepository(MainContext dbContext) : base(dbContext)
	{
	}
}