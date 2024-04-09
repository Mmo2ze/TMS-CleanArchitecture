using TMS.Domain.Common;

namespace TMS.Domain.Parents;

public record ParentId(string Value) : ValueObjectId<ParentId>(Value)
{
	public ParentId() : this(Guid.NewGuid().ToString())
	{
	}
}