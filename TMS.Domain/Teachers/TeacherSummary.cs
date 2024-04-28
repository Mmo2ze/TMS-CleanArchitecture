namespace TMS.Domain.Teachers;

public record TeacherSummary(
    TeacherId Id,
    string Name,
    string Phone,
    int StudentsCount,
    Subject Subject,
    TeacherStatus Status,
    DateOnly EndOfSubscription,
    string? Email)
{
    public static TeacherSummary FromTeacher(Teacher teacher) =>
        new(
            teacher.Id,
            teacher.Name,
            teacher.Phone,
            teacher.Students.Count,
            teacher.Subject,
            teacher.Status,
            teacher.EndOfSubscription,
            teacher.Email);
}
