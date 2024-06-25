using TMS.Domain.Common.Models;

namespace TMS.Domain.AttendanceScheduler;

public record AttendanceSchedulerId(string Value) : ValueObjectId<AttendanceSchedulerId>(Value)
{
    public AttendanceSchedulerId() : this(Guid.NewGuid().ToString())
    {
    }
}