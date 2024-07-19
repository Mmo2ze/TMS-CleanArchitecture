namespace TMS.Domain.Students;

public record StudentId(string Value) : ValueObjectId<StudentId>(Value)
{
	public StudentId() : this(Ulid.NewUlid().ToString())
	{
	}
}