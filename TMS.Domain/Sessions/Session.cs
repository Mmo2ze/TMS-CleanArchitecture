using TMS.Domain.Common.Extentions;
using TMS.Domain.Groups;
using TMS.Domain.Groups.Events;
using TMS.Domain.Sessions.Events;
using TMS.Domain.Teachers;

namespace TMS.Domain.Sessions;

public class Session : Aggregate<SessionId>
{


    public GroupId GroupId { get; private set; }
    public TeacherId TeacherId { get; private set; }
    public DayOfWeek Day { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public Grade Grade { get; private set; }

    private Session(SessionId id, GroupId groupId, TeacherId teacherId, DayOfWeek day, TimeOnly startTime,
        TimeOnly endTime, Grade grade) : base(id)
    {
        GroupId = groupId;
        TeacherId = teacherId;
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
        Grade = grade;
        RaiseDomainEvent(new SessionCreatedDomainEvent(TeacherId, EndTime, Day, Grade));
    }

    public static Session Create(GroupId groupId, TeacherId teacherId, DayOfWeek day, TimeOnly startTime,
        TimeOnly endTime, Grade grade)
    {
        startTime = startTime.SetSecondsToZero();
        endTime = endTime.SetSecondsToZero();

        return new Session(SessionId.CreateUnique(), groupId, teacherId, day, startTime, endTime, grade);
    }


    public void UpdateGrade(Grade grade)
    {
        Grade = grade;
    }
}

