using ErrorOr;
using MediatR;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Commands.Create;

public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, ErrorOr<TeacherSummary>>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTeacherCommandHandler(ITeacherRepository teacherRepository, IUnitOfWork unitOfWork)
    {
        _teacherRepository = teacherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<TeacherSummary>> Handle(CreateTeacherCommand request,
        CancellationToken cancellationToken)
    {
        // Validation
        var adminPhone = "201004714938";


        // Create teacher 
        var teacher = Teacher.Create(request.Name, request.Phone, request.Subject, request.SubscriptionPeriodInDays, adminPhone,
            request.Email);
        await _teacherRepository.AddAsync(teacher, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return  TeacherSummary.FromTeacher(teacher);

        
    }
}