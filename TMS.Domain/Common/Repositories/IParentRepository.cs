using TMS.Domain.Parents;

namespace TMS.Domain.Common.Repositories;

public interface IParentRepository
{
	Task<bool> IsParent(string phone);
	Task<Parent?> GetParentByPhone(string phone);
}