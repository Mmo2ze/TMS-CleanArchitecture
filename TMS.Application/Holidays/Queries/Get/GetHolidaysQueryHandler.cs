using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Application.Holidays.Commands.Create;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Holidays.Queries.Get;

public class GetHolidaysQueryHandler : IRequestHandler<GetHolidaysQuery, ErrorOr<PaginatedList<HolidayResult>>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IHolidayRepository _holidayRepository;

    public GetHolidaysQueryHandler(ITeacherHelper teacherHelper, IHolidayRepository holidayRepository)
    {
        _teacherHelper = teacherHelper;
        _holidayRepository = holidayRepository;
    }

    public async Task<ErrorOr<PaginatedList<HolidayResult>>> Handle(GetHolidaysQuery request,
        CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        var assistantInfo = _teacherHelper.GetAssistantInfo();
        var holidays = _holidayRepository.WhereQueryable(x => x.TeacherId == teacherId && x.GroupId == request.GroupId)
            .Include(x => x.CreatedBy)
            .Include(x => x.ModifiedBy)
            .Select(x => new HolidayResult(
                    x.Id,
                    x.StartDate,
                    x.EndDate,
                    x.GroupId,
                    x.CreatedBy == null ? assistantInfo : new AssistantInfo(x.CreatedBy.Name, x.CreatedById),
                    x.ModifiedBy == null ? assistantInfo : new AssistantInfo(x.ModifiedBy.Name, x.ModifiedById)
                )
            );
        return await holidays.PaginatedListAsync(request.Page, request.PageSize);
    }
}