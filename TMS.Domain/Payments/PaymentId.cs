namespace TMS.Domain.Payments;

public record PaymentId(string Value) : ValueObjectId<PaymentId>(Value)
{
	public PaymentId() : this(Ulid.NewUlid().ToString())
	{
	}
}