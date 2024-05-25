using ErrorOr;
using MediatR;
using TMS.Domain.Groups;

namespace TMS.Application.Groups.Commands.Delete;

public record DeleteGroupCommand(GroupId Id): IRequest<ErrorOr<string>>;