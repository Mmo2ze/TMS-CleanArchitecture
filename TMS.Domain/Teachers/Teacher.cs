using TMS.Domain.Accounts;
using TMS.Domain.Assistants;
using TMS.Domain.Groups;
using TMS.Domain.Holidays;
using TMS.Domain.Schedulers;
using TMS.Domain.Schedulers.Enums;
using TMS.Domain.Teachers.Events;

namespace TMS.Domain.Teachers;

public class Teacher : User<TeacherId>
{


    private readonly List<Assistant> _assistants = [];
    private readonly List<Account> _students = [];
    private readonly List<Group> _groups = [];
    private readonly List<Scheduler> _attendanceSchedulers = [];
    private readonly List<Holiday> _holidays = [];

    public Subject Subject { get; private set; }
    public TeacherStatus Status { get; private set; }
    public DateTime JoinDate { get; private set; }
    public DateOnly EndOfSubscription { get; private set; }
    public double DefaultDegree { get; private set; } = 10;
    public IReadOnlyList<Group> Groups => _groups.AsReadOnly();
    public IReadOnlyList<Assistant> Assistants => _assistants.AsReadOnly();
    public IReadOnlyList<Account> Students => _students.AsReadOnly();
    public IReadOnlyList<Scheduler> AttendanceSchedulers => _attendanceSchedulers.AsReadOnly();
    public IReadOnlyList<Holiday> Holidays => _holidays.AsReadOnly();
    public AutoAttendanceSchedulerOption AttendanceScheduler { get; set; } = AutoAttendanceSchedulerOption.AfterEverySession;
    public string WhatsappLink => $"https://wa.me/{Phone}";


    public void AddSubscription(int days)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow);
        if (EndOfSubscription < dateNow)
            EndOfSubscription = dateNow;
        EndOfSubscription = EndOfSubscription.AddDays(days);
        RaiseDomainEvent(new SubscriptionAddedDomainEvent(Guid.NewGuid(), Id, Name, Phone, days, EndOfSubscription));
    }


    public void AddAssistant(Assistant assistant)
    {
        _assistants.Add(assistant);
    }


    private Teacher(TeacherId id,
        string name,
        string phone,
        DateOnly endOfSubscription, Subject subject, TeacherStatus status,  string? email = null) : base(id)
    {
        Name = name;
        Email = email;
        Phone = phone;
        JoinDate = DateTime.UtcNow;
        EndOfSubscription = endOfSubscription;
        Subject = subject;
        Status = status;
    }

    public static Teacher Create(string name, string phone, Subject subject, int subscriptionPeriodInDays,
        string createdByPhone,
        string? email = null)
    {
        var endOfSubscription = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(subscriptionPeriodInDays);
        var teacher = new Teacher(TeacherId.CreateUnique(), name, phone, endOfSubscription, subject,
            TeacherStatus.Pending, email);
        teacher.RaiseDomainEvent(new TeacherCreatedDomainEvent(Guid.NewGuid(), teacher.Id.Value, teacher.Phone,
            teacher.Name, teacher.EndOfSubscription, createdByPhone));
        return teacher;
    }

    public void Update(string? requestName, string? requestPhone, Subject? subject, string? requestEmail,
        TeacherStatus? status)
    {
        if (PhoneChanged(requestPhone))
            RaiseDomainEvent(new TeacherPhoneChangedDoaminEvent(Guid.NewGuid(), Id.Value, requestPhone!));

        Name = requestName ?? Name;
        Phone = requestPhone ?? Phone;
        Subject = subject ?? Subject;
        Status = status ?? Status;
        Email = requestEmail;
    }

    private bool PhoneChanged(string? requestPhone)
    {
        return requestPhone is not null && requestPhone != Phone;
    }

    public void AddGroup(Group group)
    {
        _groups.Add(group);
    }

    public void RemoveGroup(GroupId groupId)
    {
        var group = _groups.FirstOrDefault(x => x.Id == groupId);
        if (group is null)
            return;
        _groups.Remove(group);
        RaiseDomainEvent(new GroupRemovedDomainEvent(Id, group.Id));
    }

    public override void SetHasWhatsapp(bool value)
    {
        base.SetHasWhatsapp(value);
        if(!value)
            RaiseDomainEvent(new TeacherDontHaveWhatsappDomainEvent(Id));
    }
    
    public void RemoveSchedulers()
    {
        _attendanceSchedulers.Clear();
    }

    public void AddSchedulers(List<Scheduler> newSchedulers)
    {
        _attendanceSchedulers.AddRange(newSchedulers);
    }
    
    public void AddHoliday(Holiday holiday)
    {
        _holidays.Add(holiday);
    }
    
    public void RemoveHoliday(Holiday holiday)
    {
        _holidays.Remove(holiday);
    }
    public void UpdateDefaultDegree(double degree)
    {
        DefaultDegree = degree;
    }
}