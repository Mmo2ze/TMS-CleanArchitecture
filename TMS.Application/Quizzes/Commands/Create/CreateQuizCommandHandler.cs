using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Application.Quizzes.Queries.Get;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Quizzes;

namespace TMS.Application.Quizzes.Commands.Create;

public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, ErrorOr<QuizResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateQuizCommandHandler(ITeacherHelper teacherHelper, IAccountRepository accountRepository,
        IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<QuizResult>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.FindAsync(request.AccountId, cancellationToken);
        var quiz = Quiz.Create(request.Degree, request.MaxDegree, request.AccountId,
            _teacherHelper.GetAssistantId(), _teacherHelper.GetTeacherId(), _dateTimeProvider.Today);
        account!.AddQuiz(quiz);

        return new QuizResult(
            quiz.Id,
            quiz.Degree,
            quiz.MaxDegree,
            _teacherHelper.GetAssistantInfo(),
            null,
            quiz.CreatedAt,
            null);
    }
}