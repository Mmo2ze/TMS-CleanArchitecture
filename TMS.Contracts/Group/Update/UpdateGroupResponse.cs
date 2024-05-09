using TMS.Domain.Groups;

namespace TMS.Contracts.Group.Update;

public record UpdateGroupResponse(string GroupId, string Name, Grade Grade, double BasePrice, int StudentsCount, int SessionsCount);