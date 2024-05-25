using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Groups.Commands.Delete;

public class DeleteGroupCommandHandler: IRequestHandler<DeleteGroupCommand, ErrorOr<string>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteGroupCommandHandler(IGroupRepository groupRepository, ITeacherHelper teacherHelper, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _teacherHelper = teacherHelper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<string>> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var group =await _groupRepository.GetGroup(request.Id);

        if (group is null)
        {
            return Errors.Group.NotFound;
        }
        _groupRepository.Remove(group);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return  "Group deleted successfully";
    }
}