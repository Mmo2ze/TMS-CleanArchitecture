using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
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


        var group = _groupRepository.GetQueryable()
                .Include(x => x.Sessions)
                .First(x => x.Id == request.Id);

        group.Update(request.Name, request.Grade, request.BasePrice);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return UpdateGroupResult.From(group);
    }
}