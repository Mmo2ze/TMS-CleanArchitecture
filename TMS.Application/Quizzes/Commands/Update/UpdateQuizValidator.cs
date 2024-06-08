using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Quizzes;

namespace TMS.Application.Quizzes.Commands.Update;

public class UpdateQuizValidator : AbstractValidator<UpdateQuizCommand>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IQuizRepository _quizRepository;

    public UpdateQuizValidator(ITeacherHelper teacherHelper, IQuizRepository quizRepository)
    {
        _teacherHelper = teacherHelper;
        _quizRepository = quizRepository;
        RuleFor(x => x.Degree)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Degree must be greater than or equal to 0")
            .LessThanOrEqualTo(x => x.MaxDegree)
            .WithMessage("Degree must be less than or equal to MaxDegree");
        RuleFor(x => x.Id)
            .MustAsync(BeFoundQuiz)
            .WithValidationError(Errors.Quiz.NotFound);
    }

    private Task<bool> BeFoundQuiz(QuizId arg1, CancellationToken arg2)
    {
        return _quizRepository.AnyAsync(x => x.Id == arg1 && x.TeacherId == _teacherHelper.GetTeacherId(), arg2);
    }
}