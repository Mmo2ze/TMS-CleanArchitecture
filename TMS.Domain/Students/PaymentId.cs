namespace TMS.Domain.Students;

public record PaymentId(Guid Value)
{
	public static PaymentId Create(Guid value)
	{
		return new PaymentId(value);
	}
	
	public static PaymentId CreateUnique()
	{
		return new PaymentId(Guid.NewGuid());
	}
}