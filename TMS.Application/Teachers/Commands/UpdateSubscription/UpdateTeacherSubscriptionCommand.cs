using ErrorOr;
using MediatR;
using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Commands.UpdateSubscription;

public record UpdateTeacherSubscriptionCommand(TeacherId Id,int Days):IRequest<ErrorOr<UpdateTeacherSubscriptionResult>>;
