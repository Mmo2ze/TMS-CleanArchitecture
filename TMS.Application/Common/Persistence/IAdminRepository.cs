using TMS.Domain.Admins;

namespace TMS.Application.Common.Persistence;

public interface IAdminRepository
{
	Task<bool> IsAdmin(string phone);
	Task<Admin?> GetAdminByPhone(string phone);
}