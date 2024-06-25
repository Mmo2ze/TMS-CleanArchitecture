using TMS.Domain.Accounts.Events;
using TMS.Domain.Assistants;
using TMS.Domain.Attendances;
using TMS.Domain.Groups;
using TMS.Domain.Parents;
using TMS.Domain.Quizzes;
using TMS.Domain.Quizzes.Events;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Domain.Accounts;

public class Account : Aggregate<AccountId>
{
    private readonly List<Quiz> _quizzes = [];
    private readonly List<Attendance> _attendances = [];


    private Account(AccountId id, StudentId studentId, double basePrice, GroupId groupId, TeacherId teacherId,
        ParentId? parentId) :
        base(id)
    {
        StudentId = studentId;
        BasePrice = basePrice;
        GroupId = groupId;
        TeacherId = teacherId;
        ParentId = parentId;
    }

    public IReadOnlyList<Quiz> Quizzes => _quizzes.AsReadOnly();
    public IReadOnlyList<Attendance> Attendances => _attendances.AsReadOnly();
    public StudentId StudentId { get; private set; }
    public Student Student { get; private set; } = null!;
    public ParentId? ParentId { get; private set; }

    public TeacherId TeacherId { get; private set; }

    public GroupId? GroupId { get; private set; }

    public double BasePrice { get; private set; }

    public bool HasCustomPrice { get; private set; } = true;
    public Parent? Parent { get; set; }

    public static Account Create(StudentId studentId, double basePrice, GroupId groupId, TeacherId teacherId,
        ParentId? parentId = null)
    {
        return new Account(AccountId.CreateUnique(), studentId, basePrice, groupId, teacherId, parentId);
    }


    public void Update(double? basePrice, double groupPrice, GroupId? groupId, StudentId? studentId, ParentId? parentId)
    {
        if (basePrice.HasValue && Math.Abs(BasePrice - basePrice.Value) >= .5)
        {
            BasePrice = basePrice.Value;
        }

        if (groupId != GroupId && groupId != null)
        {
            RaiseDomainEvent(new AccountMovedToGroupDomainEvent(new Guid(), this, GroupId, groupId));
        }

        GroupId = groupId ?? GroupId;
        StudentId = studentId ?? StudentId;
        ParentId = parentId ?? ParentId;
        HasCustomPrice = Math.Abs(BasePrice - groupPrice) >= .5;
    }


    public void ResetCustomPrice(double basePrice)
    {
        BasePrice = basePrice;
        HasCustomPrice = false;
    }

    public void AddQuiz(Quiz quiz)
    {
        _quizzes.Add(quiz);
        RaiseDomainEvent(new QuizCreatedDomainEvent(quiz.Id, quiz.Degree, quiz.MaxDegree, Id, quiz.AddedById));
    }

    public void RemoveQuiz(Quiz quiz)
    {
        _quizzes.Remove(quiz);
        RaiseDomainEvent(new QuizRemovedDomainEvent(quiz.Id, Id));
    }
    
    public Attendance AddAttendance(AttendanceStatus status,DateOnly date,AssistantId? assistantId)
    {
        var attendance = Attendance.Create(Id, TeacherId, assistantId, date, status);
        _attendances.Add(attendance);
        RaiseDomainEvent(new AttendanceCreatedDomainEvent(attendance.Id, attendance.Date, attendance.AccountId, attendance.TeacherId));
        return attendance;
    }
    public void RemoveAttendance(Attendance attendance)
    {
        RaiseDomainEvent(new AttendanceRemovedDomainEvent(attendance.Id, Id));
        _attendances.Remove(attendance);
    }
}