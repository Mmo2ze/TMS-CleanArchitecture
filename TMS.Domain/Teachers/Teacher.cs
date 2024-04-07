using TMS.Domain.Admins;
using TMS.Domain.Assistants;
using TMS.Domain.Students;

namespace TMS.Domain.Teachers;

public class Teacher
{
	private readonly List<Assistant> _assistants = [];
	private readonly List<Student> _students = [];
	public TeacherId Id { get; private set; }
	public string Name { get; private set; }
	public string? Email { get; private set;}
	public string Phone { get; private set; } 
	public Subject Subject { get; private set; }
	
	public DateTime JoinDate { get; private set; } 
	public DateTime EndOfSubscription { get; private set; }
	
	
	public void AddStudent(Student student)
	{
		_students.Add(student);
	}
	public void AddSubscription(TimeSpan subscription)
	{
		if(EndOfSubscription > DateTime.UtcNow)
			EndOfSubscription = DateTime.UtcNow;
		EndOfSubscription = EndOfSubscription.Add(subscription);
	}
	
	
	public void AddAssistant(Assistant assistant)
	{
			_assistants.Add(assistant);
	} 
	public IReadOnlyList<Assistant> Assistants => _assistants.AsReadOnly();
	public IReadOnlyList<Student> Students => _students.AsReadOnly();
	private Teacher(TeacherId id,
		string name,
		string phone,
		DateTime endOfSubscription, Subject subject, string? email = null)
	{
		Id = id;
		Name = name;
		Email = email;
		Phone = phone;
		JoinDate = DateTime.UtcNow;
		EndOfSubscription = endOfSubscription;
		Subject = subject;
	}
	public static Teacher Create(string name, string phone, Subject subject, DateTime endOfSubscription, string? email = null)
	{
		return new Teacher(TeacherId.CreateUnique(), name, phone, endOfSubscription, subject, email);
	}
}