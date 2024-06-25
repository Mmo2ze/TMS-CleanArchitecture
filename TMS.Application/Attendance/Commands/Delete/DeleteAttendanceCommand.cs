using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Attendances;

namespace TMS.Application.Attendance.Commands.Delete;

public record DeleteAttendanceCommand(AttendanceId Id,AccountId AccountId ) : IRequest<ErrorOr<string>>;

