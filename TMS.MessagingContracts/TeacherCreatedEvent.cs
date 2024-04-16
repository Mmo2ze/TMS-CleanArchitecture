using TMS.Domain.Teachers;

namespace TMS.MessagingContracts;

public record TeacherCreatedEvent(
    string TeacherId,
    string Name,
    string Phone);