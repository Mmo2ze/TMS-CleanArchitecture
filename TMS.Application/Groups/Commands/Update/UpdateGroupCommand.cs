using ErrorOr;
using MediatR;
using TMS.Domain.Groups;

namespace TMS.Application.Groups.Commands.Update;

public record UpdateGroupCommand(GroupId Id, string Name, Grade Grade, double BasePrice): IRequest<ErrorOr<UpdateGroupResult>>;