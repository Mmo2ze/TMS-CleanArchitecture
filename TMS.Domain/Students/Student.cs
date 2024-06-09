using TMS.Domain.Assistants;
using TMS.Domain.Common.Enums;
using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Domain.Students;

public class Student : User<StudentId>
{
    private Student():base(StudentId.CreateUnique())
    {
        
    }
    private readonly List<Teacher> _teachers = [];
    private readonly List<Attendance> _attendances = [];
    private readonly List<Payment> _payments = [];
    private readonly List<Account.Account> _accounts = [];


    public override string? Phone { get; protected set; }
    public Gender Gender { get; private set; }

    public IEnumerable<Teacher> Teachers => _teachers;
    public IEnumerable<Payment> Payments => _payments;
    public IEnumerable<Attendance> Attendances => _attendances.AsReadOnly();
    public IEnumerable<Account.Account> Accounts => _accounts.AsReadOnly();

    private Student(StudentId id, string name, Gender gender, string? email = null, string? phone = null) : base(id)
    {
        Name = name;
        Email = email;
        Gender = gender;
        Phone = phone;
    }
    
    public static Student Create(string name, Gender gender, string? email = null,
        string? phone = null)
    {
        return new Student(StudentId.CreateUnique(), name, gender, email, phone);
    }

    public void AddPayment(decimal amount, DateTime createdAt, DateOnly billDate, TeacherId teacherId,
        AssistantId? assistantId)
    {
        var payment = Payment.Create(Id, amount, createdAt, billDate, teacherId, assistantId);
        _payments.Add(payment);
    }
}