using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Assistants.Commands.Delete;

public class DeleteAssistantCommandHandler: IRequestHandler<DeleteAssistantCommand, ErrorOr<string>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAssistantRepository _assistantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAssistantCommandHandler(ITeacherHelper teacherHelper, IAssistantRepository assistantRepository, IUnitOfWork unitOfWork)
    {
        _teacherHelper = teacherHelper;
        _assistantRepository = assistantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<string>> Handle(DeleteAssistantCommand request, CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        var assistant = await _assistantRepository.GetAsync(request.Id, cancellationToken);
        _assistantRepository.Remove(assistant!);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return $"Assistant with id {request.Id.Value} has been deleted";
        
    }
}