using MediatR;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Sessions;

namespace TMS.Application.Sessions.Queries.Get;

public class GetSessionsQueryHandler : IRequestHandler<GetSessionsQuery, PaginatedList<Session>>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ITeacherHelper _teacherHelper;

    public GetSessionsQueryHandler(ITeacherHelper teacherHelper, ISessionRepository sessionRepository)
    {
        _teacherHelper = teacherHelper;
        _sessionRepository = sessionRepository;
    }

    public Task<PaginatedList<Session>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
    {
        var sessions = _sessionRepository.GetQueryable()
            .Where(s => s.TeacherId == _teacherHelper.GetTeacherId() &&
                        (request.GroupId == null || s.GroupId == request.GroupId));
        return sessions.PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}