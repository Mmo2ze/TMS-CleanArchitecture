namespace TMS.MessagingContracts.Teacher;

public record TeacherCreatedEvent(
    string TeacherPhone,
    string Name,
    DateOnly EOfSubscription,
    string CreatedByPhone) ;