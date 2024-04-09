using TMS.Domain.Common;

namespace TMS.Domain.Students;

public record PaymentId(string Value) : ValueObjectId<PaymentId>(Value)
{
	public PaymentId() : this(Guid.NewGuid().ToString())
	{
	}
}