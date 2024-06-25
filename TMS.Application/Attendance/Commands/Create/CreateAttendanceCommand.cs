using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Students;

namespace TMS.Application.Attendance.Commands.Create;

public record CreateAttendanceCommand(AccountId AccountId,  DateOnly Date,AttendanceStatus Status) : IRequest<ErrorOr<AttendanceResult>>;