namespace TMS.Domain.Teachers.Events;

public record TeacherPhoneChangedDoaminEvent (Guid Id,string TeacherId,string Phone): DomainEvent(Id);