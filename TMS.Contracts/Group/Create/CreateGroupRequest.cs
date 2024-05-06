using TMS.Domain.Groups;

namespace TMS.Contracts.Group.Create;

public record CreateGroupRequest(string Name, Grade Grade, double BasePrice);