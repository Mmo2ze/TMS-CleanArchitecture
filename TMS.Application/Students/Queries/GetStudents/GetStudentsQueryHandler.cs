using ErrorOr;
using MediatR;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Students.Queries.GetStudents;

public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, ErrorOr<PaginatedList<StudentResult>>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherHelper _teacherHelper;

    public GetStudentsQueryHandler(IStudentRepository studentRepository, ITeacherHelper teacherHelper)
    {
        _studentRepository = studentRepository;
        _teacherHelper = teacherHelper;
    }

    public async Task<ErrorOr<PaginatedList<StudentResult>>> Handle(GetStudentsQuery request,
        CancellationToken cancellationToken)
    {
        var students = _studentRepository.GetQueryable();
            if (request.PhoneRequired)
                students = students.Where(s => s.Phone != null);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            
            students = students.Where(s => s.Name.Contains(request.Search) || s.Phone.Contains(request.Search));
        }

        students = request.Sort switch
        {
            StudentSort.Name => request.Asc ? students.OrderBy(s => s.Name) : students.OrderByDescending(s => s.Name),
            _ => students
        };
        var result = await students.Select(s => StudentResult.FromStudent(s))
            .PaginatedListAsync(request.Page, request.PageSize);
        return result;
    }
}