using ErrorOr;
using MediatR;
using TMS.Application.Holidays.Commands.Create;
using TMS.Domain.Groups;
using TMS.Domain.Holidays;

namespace TMS.Application.Holidays.Commands.Update;

public record UpdateHolidayCommand(HolidayId Id, DateOnly StartDate, DateOnly EndDate, GroupId? GroupId):IRequest<ErrorOr<HolidayResult>>;