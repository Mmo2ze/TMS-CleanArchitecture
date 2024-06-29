using ErrorOr;
using MediatR;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Holidays.Commands.Delete;

public class DeleteHolidayCommandHandler : IRequestHandler<DeleteHolidayCommand, ErrorOr<string>>
{
    private readonly IHolidayRepository _holidayRepository;

    public DeleteHolidayCommandHandler(IHolidayRepository holidayRepository)
    {
        _holidayRepository = holidayRepository;
    }

    public async Task<ErrorOr<string>> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
    {
        var holiday = await _holidayRepository.FindAsync(request.Id, cancellationToken);
        if (holiday is null)
            return Errors.Holiday.NotFound;
        _holidayRepository.Remove(holiday);
        return string.Empty;
    }
}