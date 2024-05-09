using TMS.Domain.Groups;

namespace TMS.Contracts.Group.Update;

public record UpdateGroupRequest(string Name,Grade Grade,decimal BasePrice);