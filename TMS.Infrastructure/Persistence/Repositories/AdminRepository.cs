using Microsoft.EntityFrameworkCore;
using TMS.Domain.Admins;
using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Persistence.Repositories;

public class AdminRepository : IAdminRepository
{
	private readonly MainContext _dbContext;

	public AdminRepository(MainContext dbContext)
	{
		_dbContext = dbContext;
	}

	public  Task<bool> IsAdmin(string phone)
	{
		return  _dbContext.Admins.AnyAsync(admin => admin.Phone == phone);
	}

	public  Task<Admin?> GetAdminByPhone(string phone)
	{
		return  _dbContext.Admins.FirstOrDefaultAsync(admin => admin.Phone == phone);
	}

	public Task<Admin?> GetAdminById(AdminId id)
	{
		
		return  _dbContext.Admins.FirstOrDefaultAsync(admin => admin.Id == id);
	}
}