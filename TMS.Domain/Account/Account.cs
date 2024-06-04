using TMS.Domain.Account.Events;
using TMS.Domain.Common.Errors;
using TMS.Domain.Groups;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Domain.Account;

public class Account : Aggregate<AccountId>
{
    private Account(AccountId id, StudentId studentId, double basePrice, GroupId groupId, TeacherId teacherId) :
        base(id)
    {
        StudentId = studentId;
        BasePrice = basePrice;
        GroupId = groupId;
        TeacherId = teacherId;
    }

    public StudentId StudentId { get; private set; }
    public Student Student { get; private set; }

    public TeacherId TeacherId { get; private set; }

    public GroupId? GroupId { get; private set; }

    public double BasePrice { get; private set; }

    public bool HasCustomPrice { get; private set; }

    public static Account Create(StudentId studentId, double basePrice, GroupId groupId, TeacherId teacherId)
    {
        return new Account(AccountId.CreateUnique(), studentId, basePrice, groupId, teacherId);
    }


    public void Update(double? basePrice, double groupPrice, GroupId? groupId, StudentId? studentId)
    {
        if (basePrice.HasValue && Math.Abs(BasePrice - basePrice.Value) >= .5)
        {
            BasePrice = basePrice.Value;
        }

        if (groupId != GroupId && groupId != null)
        {
            RaiseDomainEvent(new AccountMovedToGroupDomainEvent(new Guid(), this,GroupId,groupId));
        }

        GroupId = groupId ?? GroupId;
        StudentId = studentId ?? StudentId;
        HasCustomPrice = Math.Abs(BasePrice - groupPrice) >= .5;
    }


    public void ResetCustomPrice(double basePrice)
    {
        BasePrice = basePrice;
        HasCustomPrice = false;
    }
}