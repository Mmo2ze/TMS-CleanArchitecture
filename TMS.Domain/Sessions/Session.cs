using TMS.Domain.Classes;
using TMS.Domain.Teachers;

namespace TMS.Domain.Sessions;

public class Session
{
    public SessionId Id { get; private set; }
    public ClassId ClassId { get; private set; }
    public TeacherId TeacherId { get; private set; }
    public DayOfWeek Day { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    
    private Session(SessionId id, ClassId classId, TeacherId teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
    {
        Id = id;
        ClassId = classId;
        TeacherId = teacherId;
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
    }
    
    public static Session Create(ClassId classId, TeacherId teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
    {
        return new Session(SessionId.CreateUnique(), classId, teacherId, day, startTime, endTime);
    }
}