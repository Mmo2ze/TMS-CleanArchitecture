using ErrorOr;
using MediatR;
using TMS.Domain.Schedulers.Enums;

namespace TMS.Application.Scheduler.Commands.Create;

public record CreateAutoSchedulerCommand(AutoAttendanceSchedulerOption SchedulerOption)
    : IRequest<ErrorOr<List<Domain.Schedulers.Scheduler>>>;