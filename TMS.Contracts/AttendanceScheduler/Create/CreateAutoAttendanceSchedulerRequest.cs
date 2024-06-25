using TMS.Domain.AttendanceSchedulers.Enums;

namespace TMS.Contracts.AttendanceScheduler.Create;

public record CreateAutoAttendanceSchedulerRequest(AutoAttendanceSchedulerOption SchedulerOption);