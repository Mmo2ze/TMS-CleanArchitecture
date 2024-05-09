namespace TMS.Domain.Sessions;

public record SessionId(string Value) : ValueObjectId<SessionId>(Value)
{
    public SessionId() : this(Guid.NewGuid().ToString())
    {
    }
}