using TMS.Domain.Common.Enums;
using TMS.Domain.Students;

namespace TMS.Application.Students.Queries.GetStudents;

public record StudentResult(
    StudentId Id,
    string Name,
    string? Phone,
    string? Email,
    Gender Gender,
    bool ? HasWhatsapp)
{
    public static StudentResult FromStudent(Student student) =>
        new(student.Id, student.Name, student.Phone, student.Email, student.Gender,student.HasWhatsapp);
};