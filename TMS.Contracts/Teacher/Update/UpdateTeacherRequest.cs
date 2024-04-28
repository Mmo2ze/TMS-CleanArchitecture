using TMS.Domain.Teachers;

namespace TMS.Contracts.Teacher.Update;

public record UpdateTeacherRequest(
    string Name,
    string Phone,
    Subject Subject,
    TeacherStatus Status,
    string? Email);