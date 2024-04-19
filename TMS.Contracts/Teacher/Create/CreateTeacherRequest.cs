using TMS.Domain.Teachers;

namespace TMS.Contracts.Teacher.Create;

public record CreateTeacherRequest(
	string Name,
	string Phone,
	Subject Subject,
	string? Email,
	int  SubscriptionPeriodInDays);