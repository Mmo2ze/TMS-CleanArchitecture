using TMS.Domain.Assistants;
using TMS.Domain.Teachers;

namespace TMS.Domain.Students;

public class Payment
{

	private Payment(PaymentId id,
		decimal amount,
		DateTime createdAt,
		DateOnly billDate, TeacherId teacherId,
		AssistantId? assistantId = null)
	{
		Id = id;
		Amount = amount;
		CreatedAt = createdAt;
		BillDate = billDate;
		TeacherId = teacherId;
		AssistantId = assistantId;
	}

	public PaymentId Id { get; private set; }
	public decimal Amount { get; private set; }
	public DateTime CreatedAt { get; private set; }
	public DateOnly BillDate { get; private set; }
	public AssistantId? AssistantId { get; private set; }
	public TeacherId TeacherId { get; private set; }

	public static Payment Create(
		decimal amount,
		DateTime createdAt,
		DateOnly billDate,
		TeacherId teacherId,
		AssistantId? assistantId)
	{
		return new Payment(PaymentId.CreateUnique(), amount, createdAt, billDate,teacherId,assistantId);
	}

}