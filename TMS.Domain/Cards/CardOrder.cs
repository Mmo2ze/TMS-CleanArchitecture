using TMS.Domain.Accounts;
using TMS.Domain.Admins;
using TMS.Domain.Teachers;

namespace TMS.Domain.Cards;

public class CardOrder : Aggregate<CardOrderId>
{
    private readonly List<AccountId> _accountIds = [];

    private CardOrder(CardOrderId id, TeacherId teacherId, DateTime createdAt,
        CardOrderStatus status, string teacherName) : base(id)
    {
        TeacherId = teacherId;
        CreatedAt = createdAt;
        Status = status;
        TeacherName = teacherName;
    }

    public IReadOnlyList<AccountId> AccountIds => _accountIds.AsReadOnly();
    public TeacherId TeacherId { get; set; }

    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string TeacherName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public AdminId? AcceptedBy { get; set; }
    public AdminId? CancelledBy { get; set; }
    public CardOrderStatus Status { get; set; }

    public static CardOrder Create(List<AccountId> accountIds, TeacherId teacherId, DateTime now, string teacherName)
    {
        var order = new CardOrder(new CardOrderId(), teacherId, now,
            CardOrderStatus.Pending, teacherName);
        order.AddAccounts(accountIds);
        return order;
    }

    public void AddAccount(AccountId accountId)
    {
        _accountIds.Add(accountId);
    }

    public void AddAccounts(List<AccountId> accountIds)
    {
        _accountIds.AddRange(accountIds);
    }

    public void Accept(AdminId adminId, DateTime now)
    {
        if (Status != CardOrderStatus.Pending) return;
        Status = CardOrderStatus.Accepted;
        AcceptedBy = adminId;
        CompletedAt = now;
    }

    public void Cancel(AdminId adminId, DateTime now)
    {
        if (Status != CardOrderStatus.Pending) return;
        Status = CardOrderStatus.Cancelled;
        CancelledBy = adminId;
        CancelledAt = now;
    }
}