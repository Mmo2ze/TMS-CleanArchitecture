using ErrorOr;
using MediatR;
using TMS.Application.Attendance.Commands.Create;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Models;

namespace TMS.Application.Attendance.Queries.Get;

public record GetAttendancesQuery(int Page, int PageSize,AccountId AccountId) : IRequest<ErrorOr<PaginatedList<AttendanceResult>>>;