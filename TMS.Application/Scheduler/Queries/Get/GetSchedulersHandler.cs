using ErrorOr;
using MediatR;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Scheduler.Queries.Get;

public class GetSchedulersHandler:IRequestHandler<GetSchedulersQuery, ErrorOr<PaginatedList<Domain.AttendanceSchedulers.Scheduler>>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly ISchedulerRepository _schedulerRepository;

    public GetSchedulersHandler(ITeacherHelper teacherHelper, ISchedulerRepository schedulerRepository)
    {
        _teacherHelper = teacherHelper;
        _schedulerRepository = schedulerRepository;
    }

    public async Task<ErrorOr<PaginatedList<Domain.AttendanceSchedulers.Scheduler>>> Handle(GetSchedulersQuery request, CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        var schedulers = _schedulerRepository.WhereQueryable(x => x.TeacherId == teacherId);

        return await schedulers.PaginatedListAsync(request.Page, request.PageSize);
    }
}