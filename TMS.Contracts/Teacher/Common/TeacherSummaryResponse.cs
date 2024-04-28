using TMS.Domain.Teachers;

namespace TMS.Contracts.Teacher.Common;

public record TeacherSummaryResponse(
    string Id,
    string Name,
    string Phone,
    int StudentsCount,
    Subject Subject,
    TeacherStatus Status,
    DateOnly EndOfSubscription
);