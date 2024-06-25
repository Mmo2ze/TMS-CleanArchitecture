using TMS.Domain.Assistants;
using TMS.Domain.Attendances;
using TMS.Domain.Students;

namespace TMS.Application.Attendance.Commands.Create;

public record AttendanceResult(
    AttendanceId Id,
    DateOnly Date,
    AttendanceStatus Status,
    AssistantInfo CreatedBy,
    AssistantInfo? UpdatedBy,
    DateTime? UpdatedAt
);