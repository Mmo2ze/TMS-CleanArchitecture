using System.Text.Json.Serialization;

namespace TMS.Contracts.Teacher.Update;

public  record UpdateTeacherPartialRequest(
    string? Name,
    string? Phone,
    string? Subject,
    string? Email);