namespace TMS.Application.Attendance.Commands.CreateScheduler;

public record CreateSchedulerCommand(AttendanceSchedulerEnum SchedulerOption);

public enum AttendanceSchedulerEnum
{
    AfterEverySession,
    AfterLastSessionOfSameGrade
}
