using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Application.Quizzes.Queries.GetDefaultDegree;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Quizzes.Commands.UpdateDefaultDegree;

public class UpdateDefaultDegreeCommandHandler: IRequestHandler<UpdateDefaultDegreeCommand, ErrorOr<GetDefaultDegreeResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly ITeacherRepository _teacherRepository;
    public UpdateDefaultDegreeCommandHandler(ITeacherHelper teacherHelper, ITeacherRepository teacherRepository)
    {
        _teacherHelper = teacherHelper;
        _teacherRepository = teacherRepository;
    }

    public async Task<ErrorOr<GetDefaultDegreeResult>> Handle(UpdateDefaultDegreeCommand request, CancellationToken cancellationToken)
    {
        var teacher  = await _teacherRepository.FindAsync(_teacherHelper.GetTeacherId(), cancellationToken);
        teacher!.UpdateDefaultDegree(request.DefaultDegree);
        return new GetDefaultDegreeResult(request.DefaultDegree);
    }
    
}