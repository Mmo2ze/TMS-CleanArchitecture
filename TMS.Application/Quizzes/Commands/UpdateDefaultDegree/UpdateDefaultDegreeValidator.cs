using FluentValidation;

namespace TMS.Application.Quizzes.Commands.UpdateDefaultDegree;

public class UpdateDefaultDegreeValidator: AbstractValidator<UpdateDefaultDegreeCommand>
{
    public UpdateDefaultDegreeValidator()
    {
        RuleFor(x => x.DefaultDegree).GreaterThanOrEqualTo(5).LessThanOrEqualTo(200);
    }
}