using TMS.Domain.Common.Models;

namespace TMS.Domain.Teachers;

public record TeacherPhoneChangedDoaminEvent (Guid Id,string TeacherId,string phone): DomainEvent(Id);