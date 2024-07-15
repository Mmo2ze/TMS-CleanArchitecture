using ErrorOr;
using MediatR;
using TMS.Application.Assistants.Commands.Create;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Assistants.Commands.Update;

public class UpdateAssistantCommandHandler: IRequestHandler<UpdateAssistantCommand, ErrorOr<AssistantDto>>
{
    private readonly IAssistantRepository _assistantRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAssistantCommandHandler(ITeacherHelper teacherHelper, IAssistantRepository assistantRepository, IUnitOfWork teacherUnitOfWork)
    {
        _teacherHelper = teacherHelper;
        _assistantRepository = assistantRepository;
        _unitOfWork = teacherUnitOfWork;
    }

    public async Task<ErrorOr<AssistantDto>> Handle(UpdateAssistantCommand request, CancellationToken cancellationToken)
    {
        var assistant = await _assistantRepository.FindAsync(request.Id, cancellationToken);
        
        assistant!.Update(request.Name, request.Phone, request.Email);
        assistant.UpdateRoles(request.Roles);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return AssistantDto.From(assistant);
    }
}