using TMS.Domain.Groups;

namespace TMS.Domain.Common.Repositories;

public interface IGroupRepository:IRepository<Group,GroupId>
{
    IQueryable<Group> GetGroups(int page, int pageSize);
    IQueryable<Group> GetGroups();
    Task<Group?> GetGroup(GroupId groupId);
    Task<bool> AnyAsync(GroupId groupId, CancellationToken cancellationToken);



    Task<Group?> ByIdAsync(GroupId groupId, CancellationToken cancellationToken = default);


}