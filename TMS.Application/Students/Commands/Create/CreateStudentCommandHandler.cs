using ErrorOr;
using MediatR;
using TMS.Application.Students.Queries.GetStudents;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Students;

namespace TMS.Application.Students.Commands.Create;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, ErrorOr<StudentResult>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateStudentCommandHandler(IStudentRepository studentRepository, IUnitOfWork unitOfWork)
    {
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<StudentResult>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        
        var student = Student.Create(request.Name,request.Gender,request.Email,request.Phone);
        _studentRepository.Add(student);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return StudentResult.FromStudent(student);
    }
}