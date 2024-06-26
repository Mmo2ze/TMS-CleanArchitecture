using ErrorOr;
using MediatR;
using TMS.Domain.Common.Models;

namespace TMS.Application.Scheduler.Queries.Get;

public record GetSchedulersQuery(int Page, int PageSize) : IRequest<ErrorOr<PaginatedList<Domain.AttendanceSchedulers.Scheduler>>>;