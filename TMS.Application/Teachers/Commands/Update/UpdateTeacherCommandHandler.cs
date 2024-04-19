using ErrorOr;
using MediatR;
using TMS.Application.Teachers.Queries.GetTeachers;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Teachers.Commands.Update;

public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand, ErrorOr<UpdateTeacherResult>>
{
    private readonly ITeacherRepository _teacherRepository;

    public UpdateTeacherCommandHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<ErrorOr<UpdateTeacherResult>> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetByIdAsync(request.TeacherId, cancellationToken);
        teacher.Update(request.Name, request.Phone, request.Email);
        await _teacherRepository.UpdateAsync(teacher, cancellationToken);
        var summary = TeacherSummary.FromTeacher(teacher);
        return new UpdateTeacherResult(summary);
    }
}