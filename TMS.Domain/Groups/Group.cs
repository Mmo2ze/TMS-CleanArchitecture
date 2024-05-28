using TMS.Domain.Groups.Events;
using TMS.Domain.Sessions;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Domain.Groups;

public class Group : Aggregate<GroupId>
{
    private readonly List<Account.Account> _students = [];
    private readonly List<Session> _sessions = [];
    public string Name { get; private set; }
    public Grade Grade { get; private set; }
    public double BasePrice { get; private set; }
    public TeacherId TeacherId { get; private set; }

    public int StudentsCount { get; private set; }
    public int SessionsCount { get; private set; }

    public IReadOnlyList<Account.Account> Students => _students.AsReadOnly();
    public IReadOnlyList<Session> Sessions => _sessions.AsReadOnly();


    private Group(GroupId id, string name, Grade grade, double basePrice, TeacherId teacherId) : base(id)
    {
        Name = name;
        Grade = grade;
        BasePrice = basePrice;
        TeacherId = teacherId;
    }

    public static Group Create(string name, Grade grade, double basePrice, TeacherId teacherId)
    {
        return new Group(GroupId.CreateUnique(), name, grade, basePrice, teacherId);
    }

    public void Update(string? name, Grade? grade, double? basePrice)
    {
        if (PriceChanged(basePrice))
        {
            BasePrice = basePrice!.Value;
            RaiseDomainEvent(new GroupPriceChangedDomainEvent(Guid.NewGuid(), Id, BasePrice));
        }

        Name = name ?? Name;
        Grade = grade ?? Grade; 
    }

    private bool PriceChanged(double? basePrice)
    {
        return basePrice.HasValue && Math.Abs(basePrice.Value - BasePrice) > 0.5;
    }

    public void AddStudent(Account.Account account)
    {
        _students.Add(account);
        StudentsCount++;
    }
    
    public void AddSession(Session session)
    {
        _sessions.Add(session);
        SessionsCount++;
    }
    
    public void RemoveStudent(Account.Account account)
    {
        _students.Remove(account);
        StudentsCount--;
        RaiseDomainEvent(new AccountRemovedFromGroupDomainEvent(Guid.NewGuid(), account.Id));
    }
    
    public void RemoveSession(Session session)
    {
        _sessions.Remove(session);
        SessionsCount--;
    }
}