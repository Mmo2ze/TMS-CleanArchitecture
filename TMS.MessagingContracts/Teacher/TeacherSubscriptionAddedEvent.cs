using TMS.Domain.Teachers;

namespace TMS.MessagingContracts.Teacher;

public record TeacherSubscriptionAddedEvent(Guid Id, TeacherId TeacherId, string Name, string Phone, int Days, DateOnly EndOfSubscription);