using ErrorOr;
using MediatR;
using TMS.Application.Attendance.Commands.Create;
using TMS.Domain.Attendances;
using TMS.Domain.Students;

namespace TMS.Application.Attendance.Commands.Update;

public record UpdateAttendanceCommand(AttendanceId Id, AttendanceStatus Status) : IRequest<ErrorOr<AttendanceResult>>;