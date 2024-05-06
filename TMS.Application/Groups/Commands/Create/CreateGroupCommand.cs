using ErrorOr;
using MediatR;
using TMS.Domain.Groups;

namespace TMS.Application.Groups.Commands.Create;

public record CreateGroupCommand(string Name, Grade Grade, double BasePrice):IRequest<ErrorOr<CreateGroupResult>>;
