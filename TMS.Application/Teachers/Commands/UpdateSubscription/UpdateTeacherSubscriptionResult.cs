using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Commands.UpdateSubscription;

public  record UpdateTeacherSubscriptionResult(TeacherId Id,DateOnly EndOfSubscription);