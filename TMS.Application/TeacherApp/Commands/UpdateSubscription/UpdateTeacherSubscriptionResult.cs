using TMS.Domain.Teachers;

namespace TMS.Application.TeacherApp.Commands.UpdateSubscription;

public  record UpdateTeacherSubscriptionResult(TeacherId Id,DateOnly EndOfSubscription);