using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;

namespace TMS.Application.Attendance.Queries.Get;

public record GetAttendancesQuery(int Month, int Year, AccountId AccountId) : IRequest<ErrorOr<GetAttendancesResult>>;