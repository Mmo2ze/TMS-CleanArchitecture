using FluentValidation;

namespace TMS.Application.TeacherApp.Commands.UpdateSubscription;

public class UpdateTeacherSubscriptionValidator:AbstractValidator<UpdateTeacherSubscriptionCommand>
{
    public UpdateTeacherSubscriptionValidator()
    {
        RuleFor(x => x.Days).GreaterThan(0);
    }
    
}