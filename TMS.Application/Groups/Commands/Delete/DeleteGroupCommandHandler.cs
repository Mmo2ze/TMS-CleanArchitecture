using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Groups.Commands.Delete;

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, ErrorOr<string>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGroupCommandHandler(ITeacherHelper teacherHelper, IUnitOfWork unitOfWork,
        ITeacherRepository teacherRepository)
    {
        _teacherHelper = teacherHelper;
        _unitOfWork = unitOfWork;
        _teacherRepository = teacherRepository;
    }

    public async Task<ErrorOr<string>> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var tId = _teacherHelper.GetTeacherId();
        var teacher = await _teacherRepository.GetQueryable()
            .Include(x => x.Groups.Where(g => g.Id == request.Id))
            .FirstOrDefaultAsync(x => x.Id == tId, cancellationToken);
        teacher!.RemoveGroup(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return "Group deleted successfully";
    }
}