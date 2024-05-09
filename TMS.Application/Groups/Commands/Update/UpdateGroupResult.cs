using TMS.Domain.Groups;

namespace TMS.Application.Groups.Commands.Update;

public record UpdateGroupResult(
    GroupId Id,
    string Name,
    Grade Grade,
    double BasePrice,
    int StudentsCount,
    int SessionsCount)
{
    public static UpdateGroupResult From(Group group) => 
    new UpdateGroupResult(group.Id, group.Name, group.Grade, group.BasePrice, group.StudentsCount, group.SessionsCount);
};