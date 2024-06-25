using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Sessions;

namespace TMS.Application.Sessions.Commands.Create;

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, ErrorOr<Session>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateSessionCommandHandler(IGroupRepository groupRepository, ITeacherHelper teacherHelper, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _teacherHelper = teacherHelper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Session>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();

        var group = await _groupRepository.FindAsync(request.GroupId, cancellationToken);

        var session = Session.Create(request.GroupId, teacherId, request.Day, request.StartTime, request.EndTime,group!.Grade);
        
        group.AddSession(session);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return session;
    }
}