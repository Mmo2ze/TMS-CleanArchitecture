using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Sessions.Commands.Delete;

public class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand, ErrorOr<string>>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGroupRepository _groupRepository;

    public DeleteSessionCommandHandler(IUnitOfWork unitOfWork, ITeacherHelper teacherHelper,
        ISessionRepository sessionRepository, IGroupRepository groupRepository)
    {
        _unitOfWork = unitOfWork;
        _teacherHelper = teacherHelper;
        _sessionRepository = sessionRepository;
        _groupRepository = groupRepository;
    }

    public async Task<ErrorOr<string>> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        var group = _groupRepository.GetQueryable()
            .Include(x => x.Sessions.Where(s => s.Id == request.Id))
            .First(g => g.Id == request.GroupId);
        group.RemoveSession(group.Sessions[0]);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return "deleted Successfully";
    }
}