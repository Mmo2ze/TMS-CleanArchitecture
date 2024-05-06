using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using Group = TMS.Domain.Groups.Group;

namespace TMS.Application.Groups.Commands.Create;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, ErrorOr<CreateGroupResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGroupCommandHandler(ITeacherHelper teacherHelper, ITeacherRepository teacherRepository, IUnitOfWork unitOfWork)
    {
        _teacherHelper = teacherHelper;
        _teacherRepository = teacherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<CreateGroupResult>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        if (teacherId == null)
        {
            return Errors.Auth.InvalidCredentials;
        }
        
        var teacher = await _teacherRepository.GetTeacher(t => t.Id == teacherId, cancellationToken);
        if (teacher == null)
        {
            return Errors.Auth.InvalidCredentials;
        }
        
        var group = Group.Create(request.Name, request.Grade, request.BasePrice, teacherId);
        teacher.AddGroup(group);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new CreateGroupResult(group.Id, group.Name, group.Grade, group.BasePrice);
    }
}