namespace TMS.Domain.Students;

public record StudentId(Guid Value)
{
	public static StudentId Create(Guid value)
	{
		return new StudentId(value);
	}
	
	public static StudentId CreateUnique()
	{
		return new StudentId(Guid.NewGuid());
	}
}