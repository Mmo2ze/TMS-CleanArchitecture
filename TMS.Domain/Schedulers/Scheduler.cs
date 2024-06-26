using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Domain.Schedulers;

public class Scheduler : Aggregate<SchedulerId>
{
    protected Scheduler(SchedulerId id,DayOfWeek day, TimeOnly firesOn, Grade? grade, TeacherId teacherId)
    {
        Id = id;
        Day = day;
        FiresOn = firesOn;
        Grade = grade;
        TeacherId = teacherId;
    }
    
    public DayOfWeek Day { get; private set; }
    public TimeOnly FiresOn { get; private set; }
    public Grade? Grade { get; private set; }
    public TeacherId TeacherId { get; private set; }
    public static Scheduler Create(DayOfWeek day, TimeOnly startTime, TeacherId teacherId,Grade? grade = null)
    {
        return new Scheduler(SchedulerId.CreateUnique(), day, startTime, grade, teacherId);
    }

    public void UpdateFiresOn(TimeOnly messageEndTime)
    {
        FiresOn = messageEndTime;
    }

    public void UpdateGrade(Grade? grade)
    {
        Grade = grade;
    }
}