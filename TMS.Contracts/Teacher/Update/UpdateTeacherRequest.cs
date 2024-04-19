namespace TMS.Contracts.Teacher.Update;

public record UpdateTeacherRequest(
    string TeacherId,
    string Name,
    string Phone,
    string? Email);