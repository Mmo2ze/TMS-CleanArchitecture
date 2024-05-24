namespace TMS.Domain.Groups;

public record GroupId(string Value) : ValueObjectId<GroupId>(Value)
{
    public GroupId() : this(Guid.NewGuid().ToString())
    {
    }
}