using Microsoft.EntityFrameworkCore;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Persistence.Repositories;

public class AssistantRepository:IAssistantRepository
{
	private readonly MainContext _dbContext;

	public AssistantRepository(MainContext dbContext)
	{
		_dbContext = dbContext;
	}

	public Task<bool> IsAssistant(string phone)
	{
		return _dbContext.Assistants.AnyAsync(a => a.Phone == phone);
	}

	public Task<Assistant?> GetAssistantByPhone(string phone)
	{
		return _dbContext.Assistants.FirstOrDefaultAsync(a => a.Phone == phone);
	}
}