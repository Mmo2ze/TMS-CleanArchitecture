using TMS.Domain.Groups.Events;
using TMS.Domain.Sessions;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Domain.Groups;

public class Group:Aggregate
{
    private readonly List<Student> _students = [];
    private readonly List<Session> _sessions = [];
    public GroupId Id { get; private set; }
    public string Name { get; private set; }
    public Grade Grade { get; private set; }
    public double BasePrice { get; private set; }
    public TeacherId TeacherId { get; private set; }
    
    public int StudentsCount { get;private set; } 
    public int SessionsCount { get;private set; }
    
    public IReadOnlyList<Student> Students => _students.AsReadOnly();
    public IReadOnlyList<Session> Sessions => _sessions.AsReadOnly();
    

    private Group(GroupId id, string name, Grade grade, double basePrice,TeacherId teacherId)
    {
        Id = id;
        Name = name;
        Grade = grade;
        BasePrice = basePrice;
        TeacherId = teacherId;
    }

    public static Group Create(string name, Grade grade,double basePrice, TeacherId teacherId)
    {
        return new Group(GroupId.CreateUnique(), name, grade,basePrice, teacherId);
    }
    
    public void Update(string? name, Grade? grade, double? basePrice)
    {
        if(PriceChanged(basePrice) )
        { 
            BasePrice = basePrice!.Value;
            RaiseDomainEvent(new ClassPriceChangedDomainEvent(Guid.NewGuid(),Id, BasePrice));
        }
        Name = name ?? Name;
        Grade = grade ?? Grade;
    }

    private bool PriceChanged(double? basePrice)
    {
        return basePrice.HasValue && Math.Abs(basePrice.Value- BasePrice) > 0.5;
    }
}