namespace TMS.Domain.Teachers;

public record TeacherId(Guid Value)
{
	public static TeacherId Create(Guid value)
	{
		return new TeacherId(value);
	}
	
	public static TeacherId CreateUnique()
	{
		return new TeacherId(Guid.NewGuid());
	}
}