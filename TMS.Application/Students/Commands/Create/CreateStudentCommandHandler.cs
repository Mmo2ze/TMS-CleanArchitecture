using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Students;

namespace TMS.Application.Students.Commands.Create;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, ErrorOr<StudentId>>
{
    private readonly IParentRepository _parentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateStudentCommandHandler(IParentRepository parentRepository, IStudentRepository studentRepository,
        ITeacherHelper teacherHelper, IUnitOfWork unitOfWork)
    {
        _parentRepository = parentRepository;
        _studentRepository = studentRepository;
        _teacherHelper = teacherHelper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<StudentId>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        if (teacherId is null)
            return Errors.Auth.InvalidCredentials;
        var parent = request.ParentId is null? null : await _parentRepository.FirstAsync(parent => parent.Id == request.ParentId, cancellationToken);
        var student = Student.Create(request.Name,request.Gender,parent,request.Email,request.Phone);
        _studentRepository.Add(student);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return student.Id;
    }
}