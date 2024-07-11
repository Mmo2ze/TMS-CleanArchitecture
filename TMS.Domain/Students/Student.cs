using TMS.Domain.Accounts;
using TMS.Domain.Common.Enums;
using TMS.Domain.Teachers;

namespace TMS.Domain.Students;

public class Student : User<StudentId>
{
    private Student():base(StudentId.CreateUnique())
    {
        
    }
    private readonly List<Teacher> _teachers = [];
    private readonly List<Account> _accounts = [];


    public override string? Phone { get; protected set; }
    public Gender Gender { get; private set; }

    public IEnumerable<Teacher> Teachers => _teachers;
    public IEnumerable<Account> Accounts => _accounts.AsReadOnly();

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


}