using TMS.Domain.Admins;

namespace TMS.Domain.Common.Repositories;

public interface IAdminRepository
{
	Task<bool> IsAdmin(string phone);
	Task<Admin?> GetAdminByPhone(string phone);
	Task<Admin?> GetAdminById(AdminId id);
}