using ErrorOr;
using MediatR;
using TMS.Domain.Groups;
using TMS.Domain.Sessions;

namespace TMS.Application.Sessions.Commands.Create;

public record CreateSessionCommand(GroupId GroupId, DayOfWeek Day, TimeOnly StartTime, TimeOnly EndTime) : IRequest<ErrorOr<Session>>;
