using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Repositories;

public class GroupRepository : Repository<Group,GroupId>,IGroupRepository
{

    public GroupRepository(MainContext context):base(context)
    {
    }

    public IQueryable<Group> GetGroups(int page, int pageSize, TeacherId teacherId)
    {
        return DbContext.Groups
            .Where(g => g.TeacherId == teacherId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsQueryable();
    }

    public IQueryable<Group> GetGroups(TeacherId teacherId)
    {
        return DbContext.Groups
            .Where(g => g.TeacherId == teacherId)
            .AsQueryable();
    }

    public Task<Group?> GetGroup(GroupId groupId)
    {
        return DbContext.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
    }

    public Task<bool> AnyAsync(GroupId groupId, TeacherId teacherId, CancellationToken cancellationToken = default)
    {
        return DbContext.Groups.AnyAsync(g => g.Id == groupId && g.TeacherId == teacherId,
            cancellationToken: cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<Group, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return DbContext.Groups.AnyAsync(predicate, cancellationToken);
    }

    public Task<Group> FirstAsync(Expression<Func<Group, bool>> predicate, CancellationToken cancellationToken)
    {
        return DbContext.Groups.FirstAsync(predicate, cancellationToken);
    }

    public Task<Group?> FirstOrDefaultAsync(Expression<Func<Group, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return DbContext.Groups.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<Group?> ByIdAsync(GroupId groupId, CancellationToken cancellationToken = default)
    {
        return DbContext.Groups.FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
    }
}