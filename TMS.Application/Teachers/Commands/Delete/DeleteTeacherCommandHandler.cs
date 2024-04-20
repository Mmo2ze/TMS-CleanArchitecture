using ErrorOr;
using MediatR;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Teachers.Commands.Delete;

public class DeleteTeacherCommandHandler: IRequestHandler<DeleteTeacherCommand, ErrorOr<DeleteTeacherResult>>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTeacherCommandHandler(ITeacherRepository teacherRepository, IUnitOfWork unitOfWork)
    {
        _teacherRepository = teacherRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<ErrorOr<DeleteTeacherResult>> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
    {
        var result = await _teacherRepository.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return result?? new ErrorOr<DeleteTeacherResult>();
    }
}