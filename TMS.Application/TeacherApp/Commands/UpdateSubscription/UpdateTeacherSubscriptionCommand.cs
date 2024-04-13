using ErrorOr;
using MediatR;
using TMS.Domain.Teachers;

namespace TMS.Application.TeacherApp.Commands.UpdateSubscription;

public record UpdateTeacherSubscriptionCommand(TeacherId Id,int Days):IRequest<ErrorOr<UpdateTeacherSubscriptionResult>>;
