namespace TMS.Domain.Schedulers;

public record SchedulerId(string Value) : ValueObjectId<SchedulerId>(Value)
{
    public SchedulerId() : this(Ulid.NewUlid().ToString())
    {
    }
}