﻿using TMS.Domain.Assistants;
using TMS.Domain.Common.Models;
using TMS.Domain.Students;

namespace TMS.Domain.Teachers;

public class Teacher : Aggregate
{
    private readonly List<Assistant> _assistants = [];
    private readonly List<Student> _students = [];
    public TeacherId Id { get; private set; }
    public string Name { get; private set; }
    public string? Email { get; private set; }
    public string Phone { get; private set; }
    public Subject Subject { get; private set; }

    public DateTime JoinDate { get; private set; }
    public DateOnly EndOfSubscription { get; private set; }
    public IReadOnlyList<Assistant> Assistants => _assistants.AsReadOnly();
    public IReadOnlyList<Student> Students => _students.AsReadOnly();


    public void AddStudent(Student student)
    {
        _students.Add(student);
    }

    public void AddSubscription(int days)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow);
        if (EndOfSubscription < dateNow)
            EndOfSubscription = dateNow;
        EndOfSubscription = EndOfSubscription.AddDays(days);

     
    }


    public void AddAssistant(Assistant assistant)
    {
        _assistants.Add(assistant);
    }
    

    private Teacher(TeacherId id,
        string name,
        string phone,
        DateOnly endOfSubscription, Subject subject, string? email = null)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        JoinDate = DateTime.UtcNow;
        EndOfSubscription = endOfSubscription;
        Subject = subject;
    }

    public static Teacher Create(string name, string phone, Subject subject, int subscriptionPeriodInDays,
        string? email = null)
    {
        var endOfSubscription = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(subscriptionPeriodInDays);
        var teacher = new Teacher(TeacherId.CreateUnique(), name, phone, endOfSubscription, subject, email);
        
        return teacher;
    }

    public void Update(string requestName, string requestPhone, string? requestEmail)
    {
        if(Phone != requestPhone)
            RaiseDomainEvent(new TeacherPhoneChangedDoaminEvent(Guid.NewGuid(),Id.Value, requestPhone));
        Name = requestName;
        Phone = requestPhone;
        Email = requestEmail;
    }
}