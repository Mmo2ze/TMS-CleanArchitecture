using TMS.Domain.Parents;

namespace TMS.Domain.Common.Repositories;

public interface IParentRepository:IRepository<Parent,ParentId>
{
	Task<bool> IsParent(string phone);
	Task<Parent?> GetParentByPhone(string phone);
}