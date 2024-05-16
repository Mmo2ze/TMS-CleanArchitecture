using System.Linq.Expressions;
using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Domain.Common.Repositories;

public interface IGroupRepository:IRepository<Group,GroupId>
{
    IQueryable<Group> GetGroups(int page, int pageSize);
    IQueryable<Group> GetGroups();
    Task<Group?> GetGroup(GroupId groupId);
    Task<bool> AnyAsync(GroupId groupId, CancellationToken cancellationToken);

    Task<bool> AnyAsync(Expression<Func<Group, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<Group> FirstAsync(Expression<Func<Group, bool>> predicate, CancellationToken cancellationToken = default);
    Task<Group?> ByIdAsync(GroupId groupId, CancellationToken cancellationToken = default);

    Task<Group?> FirstOrDefaultAsync(Expression<Func<Group, bool>> predicate,
        CancellationToken cancellationToken = default);
}