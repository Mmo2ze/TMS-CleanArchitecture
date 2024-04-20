using TMS.Domain.Teachers;

namespace TMS.Contracts.Teacher.GetTeacher;

public record GetTeacherResponse(
    string Id,
    string Name,
    string Phone,
    string? Email,
    int StudentsCount,
    bool IsActive,
    Subject Subject,
    DateOnly EndOfSubscription
    );