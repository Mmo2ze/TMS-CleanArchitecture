using TMS.Domain.Common.Models;

namespace TMS.Domain.Teachers.Events.DomainEvents;

public record TeacherCreatedDomainEvent(
    Guid Id,
    string TeacherPhone,
    string TeacherName,
    DateOnly EndOfSubscription) : DomainEvent(Id);