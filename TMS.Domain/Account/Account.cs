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

    public GroupId GroupId { get; private set; }

    public double BasePrice { get; private set; }

    public bool HasCustomPrice { get; private set; }

    public static Account Create(StudentId studentId, double basePrice, GroupId groupId, TeacherId teacherId)
    {
        return new Account(AccountId.CreateUnique(), studentId, basePrice, groupId, teacherId);
    }


    public void Update(double basePrice, double groupPrice, GroupId? groupId, StudentId? studentId)
    {
        if (Math.Abs(BasePrice - basePrice) >= .5)
        {
            SetCustomPrice(basePrice, groupPrice);
        }

        GroupId = groupId ?? GroupId;
        StudentId = studentId ?? StudentId;
    }


    public void SetCustomPrice(double price, double groupPrice)
    {
        BasePrice = price;
        if (Math.Abs(BasePrice - groupPrice) >= .5)
            HasCustomPrice = true;
    }


    public void ResetCustomPrice(double basePrice)
    {
        BasePrice = basePrice;
        HasCustomPrice = false;
    }
}