using TMS.Domain.Accounts;
using TMS.Domain.Assistants;
using TMS.Domain.Payments.Events;
using TMS.Domain.Teachers;

namespace TMS.Domain.Payments;

public class Payment : Aggregate<PaymentId>
{
    private Payment(PaymentId id, decimal amount, DateTime createdAt, DateOnly billDate, TeacherId teacherId,
        AssistantId? createdById, AccountId accountId) : base(id)
    {
        Amount = amount;
        CreatedAt = createdAt;
        BillDate = billDate;
        TeacherId = teacherId;
        CreatedById = createdById;
        AccountId = accountId;
    }

    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateOnly BillDate { get; private set; }
    public TeacherId TeacherId { get; private set; }
    public AccountId? AccountId { get; private set; }
    public AssistantId? CreatedById { get; private set; }
    public Assistant? CreatedBy { get; private set; }
    public AssistantId? ModifiedById { get; private set; }
    public Assistant? ModifiedBy { get; private set; }
    public DateTime? ModifiedAt { get; private set; }


    public static Payment Create(
        decimal amount,
        DateTime createdAt,
        DateOnly billDate,
        TeacherId teacherId,
        AssistantId? assistantId,
        AccountId accountId)
    {
        return new Payment(PaymentId.CreateUnique(), amount, createdAt, billDate, teacherId, assistantId,
            accountId);
    }

    public void Update(decimal requestAmount, DateOnly requestBillDate, DateTime now, AssistantId? getAssistantId)
    {
        Amount = requestAmount;
        BillDate = requestBillDate;
        ModifiedById = getAssistantId;
        ModifiedAt = now;
        RaiseDomainEvent(new PaymentUpdatedDomainEvent(Id, Amount, BillDate, TeacherId,AccountId!));
    }
}