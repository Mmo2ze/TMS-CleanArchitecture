using TMS.Domain.Accounts.Events;
using TMS.Domain.Assistants;
using TMS.Domain.Attendances;
using TMS.Domain.Groups;
using TMS.Domain.Parents;
using TMS.Domain.Payments;
using TMS.Domain.Quizzes;
using TMS.Domain.Quizzes.Events;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Domain.Accounts;

public class Account : Aggregate<AccountId>
{
    private readonly List<Quiz> _quizzes = [];
    private readonly List<Attendance> _attendances = [];
    private readonly List<Payment> _payments = [];


    private Account(AccountId id, StudentId studentId, double basePrice, GroupId groupId, TeacherId teacherId,
        ParentId? parentId, Grade grade) :
        base(id)
    {
        StudentId = studentId;
        BasePrice = basePrice;
        GroupId = groupId;
        TeacherId = teacherId;
        ParentId = parentId;
        Grade = grade;
    }

    public IReadOnlyList<Quiz> Quizzes => _quizzes.AsReadOnly();
    public IReadOnlyList<Attendance> Attendances => _attendances.AsReadOnly();
    public IReadOnlyList<Payment> Payments => _payments.AsReadOnly();
    public StudentId StudentId { get; private set; }
    public Student Student { get; private set; } = null!;
    public ParentId? ParentId { get; private set; }

    public TeacherId TeacherId { get; private set; }

    public GroupId? GroupId { get; private set; }
    public Group? Group { get; private set; }

    public double BasePrice { get; private set; }

    public bool HasCustomPrice { get; private set; } = true;
    public bool IsPaid { get; private set; }
    public Parent? Parent { get; set; }
    public Grade Grade { get; set; }

    public static Account Create(StudentId studentId, double basePrice, GroupId groupId, TeacherId teacherId,
        Grade grade,
        ParentId? parentId = null)
    {
        return new Account(AccountId.CreateUnique(), studentId, basePrice, groupId, teacherId, parentId, grade);
    }


    public void Update(double? basePrice, double groupPrice, GroupId? groupId, StudentId? studentId, ParentId? parentId,
        Grade? grade)
    {
        if (basePrice.HasValue && Math.Abs(BasePrice - basePrice.Value) >= .5)
        {
            BasePrice = basePrice.Value;
        }

        if (groupId != GroupId && groupId != null)
        {
            RaiseDomainEvent(new AccountMovedToGroupDomainEvent(new Guid(), this, GroupId, groupId));
        }


        Grade = grade ?? Grade;
        GroupId = groupId ?? GroupId;
        StudentId = studentId ?? StudentId;
        ParentId = parentId;
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

    public Attendance AddAttendance(AttendanceStatus status, DateOnly date, AssistantId? assistantId = null)
    {
        var attendance = Attendance.Create(Id, TeacherId, assistantId, date, status);
        _attendances.Add(attendance);
        RaiseDomainEvent(new AttendanceCreatedDomainEvent(attendance.Id, attendance.Date, attendance.AccountId,
            attendance.TeacherId));
        return attendance;
    }

    public void RemoveAttendance(Attendance attendance)
    {
        RaiseDomainEvent(new AttendanceRemovedDomainEvent(attendance.Id, Id));
        _attendances.Remove(attendance);
    }

    public void UpdateGrade(Grade grade)
    {
        Grade = grade;
    }

    public void AddPayment(Payment payment)
    {
        _payments.Add(payment);
        if (IsTheSameDate(payment))
            IsPaid = true;
        RaiseDomainEvent(new PaymentCreatedDomainEvent(payment.Id, payment.Amount, payment.BillDate,
            payment.TeacherId, payment.AccountId));
    }

    private static bool IsTheSameDate(Payment payment)
    {
        return payment.CreatedAt.Month == payment.BillDate.Month && payment.CreatedAt.Year == payment.BillDate.Year;
    }

    public void RemovePayment(Payment payment,DateTime now)
    {
        if (IsTheSameDate(payment))
            IsPaid = false;
        _payments.Remove(payment);
        RaiseDomainEvent(new PaymentRemovedDomainEvent(payment.Id, Id));
    }
}