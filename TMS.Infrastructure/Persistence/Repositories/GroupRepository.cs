using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Repositories;

public class GroupRepository : Repository<Group,GroupId>,IGroupRepository
{
    private readonly TeacherId? _teacherId;
    public GroupRepository(MainContext context, ITeacherHelper teacherHelper):base(context)
    {
        this._teacherId = teacherHelper.GetTeacherId();
    }

    public IQueryable<Group> GetGroups(int page, int pageSize)
    {
        
        return DbContext.Groups
            .Where(g => g.TeacherId == _teacherId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsQueryable();
    }

    public IQueryable<Group> GetGroups()
    {
        return DbContext.Groups
            .Where(g => g.TeacherId == _teacherId)
            .AsQueryable();
    }

    public Task<Group?> GetGroup(GroupId groupId)
    {
        return DbContext.Groups.FirstOrDefaultAsync(g => g.Id == groupId && g.TeacherId == _teacherId);
    }

    public Task<bool> AnyAsync(GroupId groupId, CancellationToken cancellationToken = default)
    {
        return DbContext.Groups.AnyAsync(g => g.Id == groupId && g.TeacherId == _teacherId,
            cancellationToken: cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<Group, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return DbContext.Groups.AnyAsync(predicate , cancellationToken);
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