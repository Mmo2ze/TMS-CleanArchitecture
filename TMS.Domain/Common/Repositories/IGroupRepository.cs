using TMS.Domain.Common.Models;
using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Domain.Common.Repositories;

public interface IGroupRepository
{
    IQueryable<Group> GetGroups(int page, int pageSize, TeacherId teacherId);
    IQueryable<Group> GetGroups(TeacherId teacherId);
    Task<Group> GetGroup(GroupId groupId);
    
}