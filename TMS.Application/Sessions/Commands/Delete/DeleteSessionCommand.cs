using ErrorOr;
using MediatR;
using TMS.Domain.Groups;
using TMS.Domain.Sessions;

namespace TMS.Application.Sessions.Commands.Delete;

public record DeleteSessionCommand(GroupId GroupId,SessionId Id) : IRequest<ErrorOr<string>>;