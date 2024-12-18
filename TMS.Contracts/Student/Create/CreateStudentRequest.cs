using TMS.Domain.Common.Enums;

namespace TMS.Contracts.Student.Create;

public record CreateStudentRequest(
    string Name,
    Gender Gender,
    string? Email = null,
    string? Phone = null);