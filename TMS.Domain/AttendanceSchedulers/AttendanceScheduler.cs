using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Domain.AttendanceSchedulers;

public class AttendanceScheduler : Aggregate<AttendanceSchedulerId>
{
    protected AttendanceScheduler(AttendanceSchedulerId id,DayOfWeek day, TimeOnly startTime, Grade? grade, TeacherId teacherId)
    {
        Id = id;
        Day = day;
        StartTime = startTime;
        Grade = grade;
        TeacherId = teacherId;
    }
    
    public DayOfWeek Day { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public Grade? Grade { get; private set; }
    public TeacherId TeacherId { get; private set; }
    public static AttendanceScheduler Create(DayOfWeek day, TimeOnly startTime, TeacherId teacherId,Grade? grade = null)
    {
        return new AttendanceScheduler(AttendanceSchedulerId.CreateUnique(), day, startTime, grade, teacherId);
    }
}