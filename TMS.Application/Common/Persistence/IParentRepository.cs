using TMS.Domain.Parents;

namespace TMS.Application.Common.Persistence;

public interface IParentRepository
{
	Task<bool> IsParent(string phone);
	Task<Parent?> GetParentByPhone(string phone);
}