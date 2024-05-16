using TMS.Domain.Common.Constrains;
using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Domain.Students;

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


    public void Update(double basePrice)
    {
        BasePrice = basePrice;
    }


    public void SetCustomPrice(double price)
    {
        BasePrice = price;
        HasCustomPrice = true;
    }

    public void ResetCustomPrice(double basePrice)
    {
        BasePrice = basePrice;
        HasCustomPrice = false;
    }
}