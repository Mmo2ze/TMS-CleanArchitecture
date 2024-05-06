using TMS.Domain.Groups;

namespace TMS.Contracts.Group.Create;

public record CreateGroupResponse( string Name, Grade Grade, double BasePrice);