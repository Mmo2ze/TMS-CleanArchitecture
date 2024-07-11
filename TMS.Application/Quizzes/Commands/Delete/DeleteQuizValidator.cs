using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Quizzes;

namespace TMS.Application.Quizzes.Commands.Delete;

public class DeleteQuizValidator: AbstractValidator<DeleteQuizCommand>
{
    private readonly IQuizRepository _quizRepository;
    private readonly ITeacherHelper _teacherHelper;
    public DeleteQuizValidator(IQuizRepository quizRepository, ITeacherHelper teacherHelper)
    {
        _quizRepository = quizRepository;
        _teacherHelper = teacherHelper;
        RuleFor(x => x.Id)
            .NotEmpty();
        
        RuleFor(x => x.Id)
            .MustAsync(BeFoundQuiz)
            .WithError(Errors.Quiz.NotFound);
    }

    private Task<bool> BeFoundQuiz(DeleteQuizCommand command,QuizId arg1, CancellationToken arg2)
    {
        return _quizRepository.AnyAsync(x => x.Id == arg1 && x.AccountId == command.AccountId && x.TeacherId == _teacherHelper.GetTeacherId(), arg2);
    }
    
    
}