using ErrorOr;
using MediatR;
using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Commands.Create;

public  record CreateTeacherCommand(
	string Name,
	string Phone,
	Subject Subject,
	string? Email,
	int SubscriptionPeriodInDays) : IRequest<ErrorOr<CreateTeacherResult>>;
	