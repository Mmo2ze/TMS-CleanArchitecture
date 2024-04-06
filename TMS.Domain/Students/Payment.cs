using TMS.Domain.Assistants;

namespace TMS.Domain.Students;

public class Payment
{
	public Payment(StudentId studentId, decimal amount, DateTime createdAt, DateOnly billDate, AssistantId? assistantId)
	{
		StudentId = studentId;
		Amount = amount;
		CreatedAt = createdAt;
		BillDate = billDate;
		AssistantId = assistantId;
	}

	public StudentId StudentId { get; private set; }
	public decimal Amount { get; private set; }
	public DateTime CreatedAt { get; private set; }
	public DateOnly BillDate { get; private set; }
	public AssistantId? AssistantId { get; private set; }
	
	public static Payment Create(StudentId studentId,
		decimal amount,
		DateTime createdAt,
		DateOnly billDate,
		AssistantId? assistantId)
	{
		return new Payment(studentId, amount, createdAt, billDate, assistantId);
	}
}