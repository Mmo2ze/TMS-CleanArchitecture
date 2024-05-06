using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Domain.Sessions;

public class Session
{
    public SessionId Id { get; private set; }
    public GroupId GroupId { get; private set; }
    public TeacherId TeacherId { get; private set; }
    public DayOfWeek Day { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    
    private Session(SessionId id, GroupId groupId, TeacherId teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
    {
        Id = id;
        GroupId = groupId;
        TeacherId = teacherId;
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
    }
    
    public static Session Create(GroupId groupId, TeacherId teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
    {
        return new Session(SessionId.CreateUnique(), groupId, teacherId, day, startTime, endTime);
    }
}