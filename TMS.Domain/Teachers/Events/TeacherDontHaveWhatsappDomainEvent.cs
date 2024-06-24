namespace TMS.Domain.Teachers.Events;

public record TeacherDontHaveWhatsappDomainEvent (TeacherId TeacherId): DomainEvent;