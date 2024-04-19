namespace TMS.Contracts.Teacher.GetTeachers;

public record GetTeachersResponse(
	IEnumerable<TeacherSummaryResponse> Teachers,
	int TotalCount,
	int Page,
	int PageSize,
	bool HasNext
	);

public record TeacherSummaryResponse(
	string Id,
	string Name,
	string Phone,
	int StudentsCount,
	string Subject,
	DateTime EndOfSubscription
	);