using TMS.Domain.Groups;

namespace TMS.Application.Groups.Queries.GetGroups;


public record GetGroupResult(GroupId GroupId, string Name, Grade Grade, double BasePrice, int StudentsCount, int SessionsCount)
{
    public static GetGroupResult FromGroup(Group group) => 
        new(group.Id, group.Name, group.Grade, group.BasePrice, group.StudentsCount, group.SessionsCount);
}