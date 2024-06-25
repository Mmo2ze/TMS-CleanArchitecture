using ErrorOr;
using MediatR;
using TMS.Domain.AttendanceSchedulers.Enums;

namespace TMS.Application.AttendanceScheduler.Commands.Create;

public record CreateAutoSchedulerCommand(AutoAttendanceSchedulerOption SchedulerOption)
    : IRequest<ErrorOr<List<Domain.AttendanceSchedulers.AttendanceScheduler>>>;