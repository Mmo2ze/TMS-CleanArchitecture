using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Teachers.Queries.GetTeacher;

public class GetTeacherQueryHandler:IRequestHandler<GetTeacherQuery,ErrorOr<GetTeacherResult>>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    public GetTeacherQueryHandler(ITeacherRepository teacherRepository, IDateTimeProvider dateTimeProvider)
    {
        _teacherRepository = teacherRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<GetTeacherResult>> Handle(GetTeacherQuery request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetTeacher(teacher => teacher.Id == request.Id);
        if (teacher is null)
            return Errors.Teacher.TeacherNotFound;
        
        var result = new GetTeacherResult(teacher.Id,
            teacher.Name,
            teacher.Phone,
            teacher.Email,
            teacher.Students.Count,
            teacher.EndOfSubscription > _dateTimeProvider.Today,
            teacher.Subject,
            teacher.EndOfSubscription);
        return result;
    }
}