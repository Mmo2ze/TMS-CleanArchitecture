using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly MainContext _context;

    public GroupRepository(MainContext context)
    {
        _context = context;
    }

    public IQueryable<Group> GetGroups(int page, int pageSize, TeacherId teacherId)
    {
        return _context.Groups
            .Where(g => g.TeacherId == teacherId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsQueryable();
    }

    public IQueryable<Group> GetGroups(TeacherId teacherId)
    {
        return _context.Groups
            .Where(g => g.TeacherId == teacherId)
            .AsQueryable();
    }

    public Task<Group?> GetGroup(GroupId groupId)
    {
        return _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
    }

    public Task<bool> AnyAsync(GroupId groupId, TeacherId teacherId, CancellationToken cancellationToken = default)
    {
        return _context.Groups.AnyAsync(g => g.Id == groupId && g.TeacherId == teacherId,
            cancellationToken: cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<Group, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return _context.Groups.AnyAsync(predicate, cancellationToken);
    }

    public Task<Group> FirstAsync(Expression<Func<Group, bool>> predicate, CancellationToken cancellationToken)
    {
        return _context.Groups.FirstAsync(predicate, cancellationToken);
    }

    public Task<Group?> FirstOrDefaultAsync(Expression<Func<Group, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return _context.Groups.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<Group?> ByIdAsync(GroupId groupId, CancellationToken cancellationToken = default)
    {
        return _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
    }
}