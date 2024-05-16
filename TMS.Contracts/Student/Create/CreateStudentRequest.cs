using TMS.Domain.Common.Enums;

namespace TMS.Contracts.Student.Create;

public record CreateStudentRequest(
    string Name,
    Gender Gender,
    Guid? ParentId = null,
    string? Email = null,
    string? Phone = null);