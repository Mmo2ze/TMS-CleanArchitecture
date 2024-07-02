using TMS.Domain.Accounts;
using TMS.Domain.Admins;
using TMS.Domain.Teachers;

namespace TMS.Domain.Cards;

public class CardOrder : Aggregate<CardOrderId>
{
    private readonly List<Account> _accounts = [];

    private CardOrder(CardOrderId id, TeacherId teacherId, DateTime createdAt,
        CardOrderStatus status, string teacherName) : base(id)
    {
        TeacherId = teacherId;
        CreatedAt = createdAt;
        Status = status;
        TeacherName = teacherName;
    }

    public IReadOnlyList<Account> Accounts => _accounts.AsReadOnly();
    public TeacherId TeacherId { get; set; }

    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string TeacherName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public AdminId? AcceptedBy { get; set; }
    public DateTime? CancelledAt { get; set; }
    public AdminId? CancelledBy { get; set; }
    public DateTime? CompletedAt { get; set; }
    public AdminId? CompletedBy { get; set; }
    public CardOrderStatus Status { get; set; }

    public static CardOrder Create(List<Account> accounts, TeacherId teacherId, DateTime now, string teacherName)
    {
        var order = new CardOrder(CardOrderId.CreateUnique(), teacherId, now,
            CardOrderStatus.Pending, teacherName);
        order.AddAccounts(accounts);
        return order;
    }

    public void AddAccount(Account accountId)
    {
        _accounts.Add(accountId);
    }

    public void AddAccounts(List<Account> accountIds)
    {
        _accounts.AddRange(accountIds);
    }

    public void UpdateAccounts(List<Account> accountIds)
    {
        _accounts.Clear();
        _accounts.AddRange(accountIds);
    }


    public void UpdateStatus(AdminId adminId, CardOrderStatus status, DateTime now)
    {
        if (Status != CardOrderStatus.Pending) return;
        Status = status;
        switch (status)
        {
            case CardOrderStatus.Processing:
                AcceptedBy = adminId;
                AcceptedAt = now;
                break;
            case CardOrderStatus.Cancelled:
                CancelledBy = adminId;
                CancelledAt = now;
                break;
            case CardOrderStatus.Completed:
                CompletedBy = adminId;
                CompletedAt = now;
                break;
        }
    }
}