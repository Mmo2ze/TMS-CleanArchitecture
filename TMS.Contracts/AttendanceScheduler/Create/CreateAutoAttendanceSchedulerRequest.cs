using TMS.Domain.Schedulers.Enums;

namespace TMS.Contracts.AttendanceScheduler.Create;

public record CreateAutoAttendanceSchedulerRequest(AutoAttendanceSchedulerOption SchedulerOption);