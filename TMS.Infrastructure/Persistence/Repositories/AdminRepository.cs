using Microsoft.EntityFrameworkCore;
using TMS.Domain.Admins;
using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Persistence.Repositories;

public class AdminRepository : Repository<Admin,AdminId> ,IAdminRepository
{

	public AdminRepository(MainContext dbContext) : base(dbContext)
	{
	}


	public  Task<bool> IsAdmin(string phone)
	{
		return  DbContext.Admins.AnyAsync(admin => admin.Phone == phone);
	}

	public  Task<Admin?> GetAdminByPhone(string phone)
	{
		return  DbContext.Admins.FirstOrDefaultAsync(admin => admin.Phone == phone);
	}

	public Task<Admin?> GetAdminById(AdminId id)
	{
		
		return  DbContext.Admins.FirstOrDefaultAsync(admin => admin.Id == id);
	}

}