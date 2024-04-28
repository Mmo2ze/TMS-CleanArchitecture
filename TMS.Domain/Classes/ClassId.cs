using TMS.Domain.Common.Models;

namespace TMS.Domain.Classes;

public record ClassId(string Value) : ValueObjectId<ClassId>(Value)
{
    public ClassId() : this(Guid.NewGuid().ToString())
    {
    }
}