using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Repositories;

public class GroupRepository: IGroupRepository
{
    private readonly MainContext _context;

    public GroupRepository(MainContext context)
    {
        _context = context;
    }

    public IQueryable<Group> GetGroups(int page, int pageSize, TeacherId teacherId)
    {
        return  _context.Groups
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

    public async Task<Group?> GetGroup(GroupId groupId)
    {
        return  _context.Groups.FirstOrDefault(g => g.Id == groupId);
    }
}