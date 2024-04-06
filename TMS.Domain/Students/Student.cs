using TMS.Domain.Teachers;

namespace TMS.Domain.Students;

public class Student
{


	private readonly List<Teacher> _teachers = [];
	private readonly List<Payment> _payments = [];

	public StudentId Id { get; private set; }
	public string Name { get; private set; }
	public string? Email { get; private set; }
	public string? Phone { get; private set; }
	public Gender Gender { get; private set; }
	public IReadOnlyList<Teacher> Teachers => _teachers.AsReadOnly();
	public IReadOnlyList<Payment> Payments => _payments.AsReadOnly();

	private Student(StudentId id, string name, Gender gender, string? email = null, string? phone = null)
	{
		Id = id;
		Name = name;
		Email = email;
		Gender = gender;
		Phone = phone;
	}

	public static Student Create(string name, Gender gender, string? email = null, string? phone = null)
	{
		return new Student(StudentId.CreateUnique(), name, gender, email, phone);
	}
}