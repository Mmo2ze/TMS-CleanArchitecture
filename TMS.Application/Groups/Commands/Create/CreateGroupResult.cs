using TMS.Domain.Groups;

namespace TMS.Application.Groups.Commands.Create;

public record CreateGroupResult(GroupId Id, string Name, Grade Grade, double BasePrice);
