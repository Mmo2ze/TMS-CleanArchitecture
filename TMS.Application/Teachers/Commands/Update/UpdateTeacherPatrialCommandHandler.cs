using ErrorOr;
using MediatR;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Commands.Update;

public class UpdateTeacherPartialCommandHandler :
    IRequestHandler<UpdateTeacherPartialCommand, ErrorOr<TeacherSummary>>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateTeacherPartialCommandHandler(ITeacherRepository teacherRepository, IUnitOfWork unitOfWork)
    {
        _teacherRepository = teacherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<TeacherSummary>> Handle(UpdateTeacherPartialCommand request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetByIdAsync(request.TeacherId, cancellationToken);
        if (teacher == null)
            return Errors.Teacher.TeacherNotFound;
        teacher.Update(request.Name, request.Phone, request.Subject, request.Email, request.Status);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return TeacherSummary.FromTeacher(teacher);

    }
}