using TMS.Domain.Common.Models;

namespace TMS.Domain.Students;

public record StudentId(string Value) : ValueObjectId<StudentId>(Value)
{
	public StudentId() : this(Guid.NewGuid().ToString())
	{
	}
}