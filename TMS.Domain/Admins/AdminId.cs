namespace TMS.Domain.Admins;

public record AdminId(Guid Value)
{
	public static AdminId Create(Guid value)
	{
		return new AdminId(value);
	}
	
	public static AdminId CreateUnique()
	{
		return new AdminId(Guid.NewGuid());
	}
}