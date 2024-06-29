using ErrorOr;
using MediatR;
using TMS.Application.Holidays.Commands.Create;
using TMS.Domain.Common.Models;
using TMS.Domain.Groups;

namespace TMS.Application.Holidays.Queries.Get;

public record GetHolidaysQuery(
    int Page,
    int PageSize,
    GroupId? GroupId):
    IRequest<ErrorOr<PaginatedList<HolidayResult>>>;