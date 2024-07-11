using ErrorOr;
using MassTransit.Initializers;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Quizzes.Queries.GetDefaultDegree;

public class GetDefaultDegreeQueryHandler : IRequestHandler<GetDefaultDegreeQuery, ErrorOr<GetDefaultDegreeResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly ITeacherRepository _teacherRepository;

    public GetDefaultDegreeQueryHandler(ITeacherRepository teacherRepository, ITeacherHelper teacherHelper)
    {
        _teacherRepository = teacherRepository;
        _teacherHelper = teacherHelper;
    }

    public async Task<ErrorOr<GetDefaultDegreeResult>> Handle(GetDefaultDegreeQuery request, CancellationToken cancellationToken)
    {
        var degree = await _teacherRepository.FindAsync(_teacherHelper.GetTeacherId(), cancellationToken)
            .Select(x => x!.DefaultDegree);
        return new GetDefaultDegreeResult(degree);
    }
}