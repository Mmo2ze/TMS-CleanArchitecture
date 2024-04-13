using ErrorOr;
using MediatR;
using TMS.Domain.Teachers;

namespace TMS.Application.TeacherApp.Commands.AddTeacher;

public record AddTeacherCommand(
	string Name,
	string Phone,
	Subject Subject,
	string? Email,
	int SubscriptionPeriodInDays) : IRequest<ErrorOr<AddTeacherResult>>;