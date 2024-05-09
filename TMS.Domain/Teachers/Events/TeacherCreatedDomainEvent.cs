namespace TMS.Domain.Teachers.Events;

public record TeacherCreatedDomainEvent(
    Guid Id,
    string TeacherId,
    string TeacherPhone,
    string TeacherName,
    DateOnly EndOfSubscription,
    string CretedByPhone) : DomainEvent(Id);