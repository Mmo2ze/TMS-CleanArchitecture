using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Application.Holidays.Commands.Create;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Holidays.Commands.Update;

public class UpdateHolidayCommandHandler : IRequestHandler<UpdateHolidayCommand, ErrorOr<HolidayResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IHolidayRepository _holidayRepository;

    public UpdateHolidayCommandHandler(ITeacherHelper teacherHelper, IHolidayRepository holidayRepository)
    {
        _teacherHelper = teacherHelper;
        _holidayRepository = holidayRepository;
    }


    public async Task<ErrorOr<HolidayResult>> Handle(UpdateHolidayCommand request, CancellationToken cancellationToken)
    {
        var holiday = await _holidayRepository.GetQueryable()
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.TeacherId == _teacherHelper.GetTeacherId(),
                cancellationToken: cancellationToken);
        if (holiday is null)
            return Errors.Holiday.NotFound;

        holiday.Update(request.StartDate, request.EndDate, request.GroupId, _teacherHelper.GetAssistantId());
        return new HolidayResult(
            holiday.Id,
            holiday.StartDate,
            holiday.EndDate,
            holiday.GroupId,
            holiday.CreatedBy != null
                ? new AssistantInfo(holiday.CreatedBy.Name, holiday.CreatedBy.Id)
                : _teacherHelper.TeacherInfo(),
            _teacherHelper.GetAssistantInfo()
        );
    }
}