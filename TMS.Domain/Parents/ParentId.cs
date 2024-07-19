namespace TMS.Domain.Parents;

public record ParentId(string Value) : ValueObjectId<ParentId>(Value)
{
	public ParentId() : this(Ulid.NewUlid().ToString())
	{
	}
}