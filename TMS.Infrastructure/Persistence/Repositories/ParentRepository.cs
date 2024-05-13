using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Parents;

namespace TMS.Infrastructure.Persistence.Repositories;

public class ParentRepository:Repository<Parent,ParentId>,IParentRepository
{

	public ParentRepository(MainContext dbContext):base(dbContext)
	{
	
	}

	public Task<bool> IsParent(string phone)
	{

		return DbContext.Parents.AnyAsync(p => p.Phone == phone);
	}

	public Task<Parent?> GetParentByPhone(string phone)
	{
		return DbContext.Parents.FirstOrDefaultAsync(p => p.Phone == phone);
	}
}