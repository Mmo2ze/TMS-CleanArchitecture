using TMS.Domain.Teachers;

namespace TMS.Application.TeacherApp.Queries.GetTeachers;

public record GetTeachersResult(
	IEnumerable<TeacherSummary> Teachers,
	int TotalCount,
	int Page,
	int PageSize,
	bool HasNext);

public record TeacherSummary(
	TeacherId Id,
	string Name,
	string Phone,
	int StudentsCount,
	Subject Subject,
	DateOnly EndOfSubscription);
