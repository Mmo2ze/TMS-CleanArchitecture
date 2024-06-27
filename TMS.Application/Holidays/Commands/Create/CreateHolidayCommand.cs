using ErrorOr;
using MediatR;
using TMS.Domain.Groups;

namespace TMS.Application.Holidays.Commands.Create;

public record CreateHolidayCommand(DateOnly StartDate, DateOnly EndDate, GroupId? GroupId) : IRequest<ErrorOr<HolidayResult>>;

