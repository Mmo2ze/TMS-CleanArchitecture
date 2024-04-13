using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Parents;

namespace TMS.Infrastructure.Persistence.Repositories;

public class ParentRepository:IParentRepository
{
	private readonly MainContext _dbContext;

	public ParentRepository(MainContext dbContext)
	{
		_dbContext = dbContext;
	}

	public Task<bool> IsParent(string phone)
	{

		return _dbContext.Parents.AnyAsync(p => p.Phone == phone);
	}

	public Task<Parent?> GetParentByPhone(string phone)
	{
		return _dbContext.Parents.FirstOrDefaultAsync(p => p.Phone == phone);
	}
}