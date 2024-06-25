using TMS.Domain.Groups;

namespace TMS.Contracts.AttendanceScheduler.Create;

public record AttendanceSchedulerResponse(string Id ,DayOfWeek Day, TimeOnly StartTime, Grade? Grade);