using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Enums;
using TMS.Domain.Parents;
using TMS.Domain.Teachers;

namespace TMS.Domain.Students;

public class Student
{


	private readonly List<Teacher>  _teachers = [];
	private readonly List<Attendance> _attendances = [];
	private readonly List<Payment> _payments = [];

	public StudentId Id { get; private set; }
	public string Name { get; private set; }
	public string? Email { get; private set; }
	public string? Phone { get; private set; }
	public Gender Gender { get; private set; }
	public Parent? Parent { get; private set; }
	
	public IEnumerable<Teacher> Teachers => _teachers;
	public IEnumerable<Payment> Payments => _payments;
	public IEnumerable<Attendance> Attendances => _attendances.AsReadOnly();

	private Student(StudentId id, string name, Gender gender, string? email = null, string? phone = null)
	{
		Id = id;
		Name = name;
		Email = email;
		Gender = gender;
		Phone = phone;
	}

	public static Student Create(string name, Gender gender,Parent? parent = null , string? email = null, string? phone = null)
	{
		return new Student(StudentId.CreateUnique(), name, gender,email, phone);
	}
	
	public void AddPayment(decimal amount, DateTime createdAt, DateOnly billDate, TeacherId teacherId,AssistantId? assistantId)
	{
		var payment = Payment.Create(Id, amount, createdAt, billDate,teacherId, assistantId);
		_payments.Add(payment);
	}
	
	
}