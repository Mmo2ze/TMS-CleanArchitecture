using TMS.Domain.Accounts;
using TMS.Domain.Groups.Events;
using TMS.Domain.Sessions;
using TMS.Domain.Sessions.Events;
using TMS.Domain.Teachers;

namespace TMS.Domain.Groups;

public class Group : Aggregate<GroupId>
{
    private readonly List<Account> _accounts = [];
    private readonly List<Session> _sessions = [];
    public string Name { get; private set; }
    public Grade Grade { get; private set; }
    public double BasePrice { get; private set; }
    public TeacherId TeacherId { get; private set; }

    public int StudentsCount { get; private set; }
    public int SessionsCount { get; private set; }

    public IReadOnlyList<Account> Accounts => _accounts.AsReadOnly();
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

        if (Grade != grade && grade.HasValue)
        {
            RaiseDomainEvent(new GroupGradeChangedDomainEvent(TeacherId,Id, Grade, grade.Value));
            foreach (var session in _sessions)
            {
                session.UpdateGrade(grade.Value);
            }

            foreach (var account in _accounts)
            {
                account.UpdateGrade(grade.Value);
            }
            Grade = grade.Value;

        }

        Name = name ?? Name;
    }

    private bool PriceChanged(double? basePrice)
    {
        return basePrice.HasValue && Math.Abs(basePrice.Value - BasePrice) > 0.5;
    }

    public void AddStudent(Account account)
    {
        _accounts.Add(account);
        StudentsCount++;
    }

    public void AddSession(Session session)
    {
        _sessions.Add(session);
        SessionsCount++;
        RaiseDomainEvent(new SessionCreatedDomainEvent(TeacherId, session.EndTime, session.Day, Grade));
    }

    public void RemoveStudent(Account account)
    {
        _accounts.Remove(account);
        StudentsCount--;
        RaiseDomainEvent(new AccountRemovedFromGroupDomainEvent(Guid.NewGuid(), account.Id));
    }

    public void RemoveSession(Session session)
    {
        _sessions.Remove(session);
        SessionsCount--;
        RaiseDomainEvent(new SessionRemovedFromGroupDomainEvent(Guid.NewGuid(), TeacherId, session.Id, session.EndTime,
            session.Day,Grade));
    }
}