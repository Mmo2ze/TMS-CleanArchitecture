using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Queries.GetTeachers;

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
	DateOnly EndOfSubscription)
{
	public static TeacherSummary FromTeacher(Teacher teacher) =>
		new TeacherSummary(
			teacher.Id,
			teacher.Name,
			teacher.Phone,
			teacher.Students.Count,
			teacher.Subject,
			teacher.EndOfSubscription);
};
