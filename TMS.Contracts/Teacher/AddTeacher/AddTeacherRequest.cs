using TMS.Domain.Teachers;

namespace TMS.Contracts.Teacher.AddTeacher;

public record AddTeacherRequest(
	string Name,
	string Phone,
	Subject Subject,
	string? Email,
	int  SubscriptionPeriodInDays);