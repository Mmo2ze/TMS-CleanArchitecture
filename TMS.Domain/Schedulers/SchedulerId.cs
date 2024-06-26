namespace TMS.Domain.AttendanceSchedulers;

public record SchedulerId(string Value) : ValueObjectId<SchedulerId>(Value)
{
    public SchedulerId() : this(Guid.NewGuid().ToString())
    {
    }
}