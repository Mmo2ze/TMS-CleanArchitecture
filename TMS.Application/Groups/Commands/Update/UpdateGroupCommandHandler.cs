using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Groups.Commands.Update;

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, ErrorOr<UpdateGroupResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGroupRepository _groupRepository;

    public UpdateGroupCommandHandler(ITeacherHelper teacherHelper,
        IUnitOfWork unitOfWork, IGroupRepository groupRepository)
    {
        _teacherHelper = teacherHelper;
        _unitOfWork = unitOfWork;
        _groupRepository = groupRepository;
    }

    public async Task<ErrorOr<UpdateGroupResult>> Handle(UpdateGroupCommand request,
        CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        if (teacherId == null)
            return Errors.Auth.InvalidCredentials;

        var group = await _groupRepository.ByIdAsync(request.Id, cancellationToken);

        group!.Update(request.Name, request.Grade, request.BasePrice);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return UpdateGroupResult.From(group);
    }
}