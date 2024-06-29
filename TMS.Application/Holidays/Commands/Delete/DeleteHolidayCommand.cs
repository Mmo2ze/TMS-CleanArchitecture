using ErrorOr;
using MediatR;
using TMS.Domain.Holidays;

namespace TMS.Application.Holidays.Commands.Delete;

public record DeleteHolidayCommand(HolidayId Id):IRequest<ErrorOr<string>>;