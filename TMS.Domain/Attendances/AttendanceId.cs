namespace TMS.Domain.Attendances;

public record AttendanceId(string Value) : ValueObjectId<AttendanceId>(Value)
{
    public AttendanceId() : this(Guid.NewGuid().ToString())
    {
    }
}