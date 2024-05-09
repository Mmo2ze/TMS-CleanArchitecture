using System.Linq.Expressions;
using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Domain.Common.Repositories;

public interface IGroupRepository
{
    IQueryable<Group> GetGroups(int page, int pageSize, TeacherId teacherId);
    IQueryable<Group> GetGroups(TeacherId teacherId);
    Task<Group?> GetGroup(GroupId groupId);
    Task<bool> AnyAsync(GroupId groupId, TeacherId teacherId, CancellationToken cancellationToken);

    Task<bool> AnyAsync(Expression<Func<Group, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<Group> FirstAsync(Expression<Func<Group, bool>> predicate, CancellationToken cancellationToken = default);
    Task<Group?> ByIdAsync(GroupId groupId, CancellationToken cancellationToken = default);

    Task<Group?> FirstOrDefaultAsync(Expression<Func<Group, bool>> predicate,
        CancellationToken cancellationToken = default);
}