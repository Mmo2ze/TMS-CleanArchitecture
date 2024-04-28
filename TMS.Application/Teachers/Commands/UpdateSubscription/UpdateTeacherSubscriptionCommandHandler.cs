using ErrorOr;
using MediatR;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Teachers.Commands.UpdateSubscription;

public class UpdateTeacherSubscriptionCommandHandler:IRequestHandler<UpdateTeacherSubscriptionCommand, ErrorOr<UpdateTeacherSubscriptionResult>>
{
    private readonly ITeacherRepository _teacherRepository;
    public UpdateTeacherSubscriptionCommandHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<ErrorOr<UpdateTeacherSubscriptionResult>> Handle(UpdateTeacherSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetTeacher(t => t.Id == request.Id, cancellationToken);
        if (teacher is null)
            return Errors.Teacher.TeacherNotFound;
        teacher.AddSubscription(request.Days);
        await _teacherRepository.UpdateTeacher(teacher, cancellationToken);
        return new UpdateTeacherSubscriptionResult(teacher.Id, teacher.EndOfSubscription);
    }
}