using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Models;

namespace TMS.Application.Attendance.Queries.Get;

public record GetAttendancesQuery(int Month, int Year, AccountId AccountId) : IRequest<ErrorOr<GetAttendancesResult>>;