using MediatR;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.TeacherApp.Queries.GetTeachers;

public class GetTeachersQueryHandler : IRequestHandler<GetTeachersQuery, GetTeachersResult>
{
	private readonly ITeacherRepository _teacherRepository;

	public GetTeachersQueryHandler(ITeacherRepository teacherRepository)
	{
		_teacherRepository = teacherRepository;
	}

	public async Task<GetTeachersResult> Handle(GetTeachersQuery request, CancellationToken cancellationToken)
	{
		var teachers = await _teacherRepository.GetTeachers(request.Page, request.PageSize);
		var teacherSummaries = teachers.Select(t => new TeacherSummary
			(t.Id, t.Name, t.Phone, t.Students.Count,t.Subject, t.EndOfSubscription));
		var result = new GetTeachersResult(teacherSummaries, teachers.Count, request.PageSize, request.Page, true);
		return result;
	}
}