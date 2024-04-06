namespace TMS.Domain.Parents;

public record ParentId(Guid Value)
{
	public static ParentId Create(Guid value)
	{
		return new ParentId(value);
	}
	
	public static ParentId CreateUnique()
	{
		return new ParentId(Guid.NewGuid());
	}
}