namespace TMS.Domain.Attendances;

public record AttendanceId(string Value) : ValueObjectId<AttendanceId>(Value)
{
    public AttendanceId() : this(Ulid.NewUlid().ToString())
    {
    }
}