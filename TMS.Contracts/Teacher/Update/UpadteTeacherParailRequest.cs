using TMS.Domain.Teachers;

namespace TMS.Contracts.Teacher.Update;

public  record UpdateTeacherPartialRequest(
    string? Name,
    string? Phone,
    string? Subject,
    string? Email,
    TeacherStatus? Status);