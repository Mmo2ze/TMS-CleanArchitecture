using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Assistants.Commands.Create;

public class CreateAssistantCommandHandler : IRequestHandler<CreateAssistantCommand, ErrorOr<AssistantDto>>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAssistantCommandHandler(ITeacherRepository teacherRepository, ITeacherHelper teacherHelper, IUnitOfWork unitOfWork)
    {
        _teacherRepository = teacherRepository;
        _teacherHelper = teacherHelper;
        _unitOfWork = unitOfWork;
    }
 
    public async Task<ErrorOr<AssistantDto>> Handle(CreateAssistantCommand request,
        CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        if (teacherId is null)
        {
            return Errors.Auth.InvalidCredentials;
        }

        var teacher = await _teacherRepository.GetTeacher(t => t.Id == teacherId, cancellationToken);
        if (teacher is null)
        {
            return Errors.Teacher.TeacherNotFound;
        }

        var assistant = Assistant.Create(request.Name, request.Phone,teacherId, request.Email);
        assistant.UpdateRoles(request.Roles);
        teacher.AddAssistant(assistant);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AssistantDto.From(assistant);
    }
}





