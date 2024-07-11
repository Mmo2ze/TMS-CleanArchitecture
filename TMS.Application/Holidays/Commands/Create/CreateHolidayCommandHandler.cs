using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Holidays;

namespace TMS.Application.Holidays.Commands.Create;

public class CreateHolidayCommandHandler:IRequestHandler<CreateHolidayCommand,ErrorOr<HolidayResult>>
{
    
    private readonly ITeacherHelper _teacherHelper;
    private readonly ITeacherRepository _teacherRepository;

    public CreateHolidayCommandHandler(ITeacherHelper teacherHelper, ITeacherRepository teacherRepository)
    {
        _teacherHelper = teacherHelper;
        _teacherRepository = teacherRepository;
    }

    public async Task<ErrorOr<HolidayResult>> Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
    {

        var teacher =await _teacherRepository.FindAsync(_teacherHelper.GetTeacherId(), cancellationToken);
        
        var holiday =  Holiday.Create(teacher!.Id,request.GroupId,request.StartDate, request.EndDate,_teacherHelper.GetAssistantId());
        teacher.AddHoliday(holiday);
        return new HolidayResult(
            holiday.Id,
            holiday.StartDate,
            holiday.EndDate,
            holiday.GroupId,
            _teacherHelper.GetAssistantInfo(),
            null
            );
        
    }
}