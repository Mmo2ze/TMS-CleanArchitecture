using TMS.Domain.Common.Models;

namespace TMS.Domain.Teachers;

public record TeacherId(string Value) : ValueObjectId<TeacherId>(Value)
{
	public TeacherId() : this(Guid.NewGuid().ToString())
	{
	}
};