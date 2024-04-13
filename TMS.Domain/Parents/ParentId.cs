using TMS.Domain.Common;
using TMS.Domain.Common.Models;

namespace TMS.Domain.Parents;

public record ParentId(string Value) : ValueObjectId<ParentId>(Value)
{
	public ParentId() : this(Guid.NewGuid().ToString())
	{
	}
}