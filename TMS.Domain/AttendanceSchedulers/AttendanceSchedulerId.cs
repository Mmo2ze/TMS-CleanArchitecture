namespace TMS.Domain.AttendanceSchedulers;

public record AttendanceSchedulerId(string Value) : ValueObjectId<AttendanceSchedulerId>(Value)
{
    public AttendanceSchedulerId() : this(Guid.NewGuid().ToString())
    {
    }
}