using MediatR;
using TMS.Application.Common.Mapping;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Queries.GetTeachers;

public class GetTeachersQueryHandler : IRequestHandler<GetTeachersQuery, PaginatedList<TeacherSummary>>
{
	private readonly ITeacherRepository _teacherRepository;

	public GetTeachersQueryHandler(ITeacherRepository teacherRepository)
	{
		_teacherRepository = teacherRepository;
	}

	public async Task<PaginatedList<TeacherSummary>> Handle(GetTeachersQuery request, CancellationToken cancellationToken)
	{
		var teachers = await _teacherRepository.GetTeachers( cancellationToken);
		var teachersSummaries = await teachers.
			Select(t=>TeacherSummary.FromTeacher(t))
			.PaginatedListAsync(request.Page,request.PageSize);
		return  teachersSummaries;
	}
}