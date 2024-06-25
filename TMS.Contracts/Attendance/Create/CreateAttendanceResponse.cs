using TMS.Domain.Assistants;
using TMS.Domain.Students;

namespace TMS.Contracts.Attendance.Create;

public record AttendanceResponse( string Id,
    DateOnly Date,
    AttendanceStatus Status,
    AssistantInfo CreatedBy,
    AssistantInfo? UpdatedBy,
    DateTime? UpdatedAt
);