using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Quizzes;

namespace TMS.Application.Quizzes.Create;

public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, ErrorOr<QuizId>>
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

    public async Task<ErrorOr<QuizId>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAsync(request.AccountId, cancellationToken);
        var quiz = Quiz.Create(request.Degree, request.MaxDegree, request.AccountId,
            _teacherHelper.GetAssistantId(), _teacherHelper.GetTeacherId(), _dateTimeProvider.Today);
        account!.AddQuiz(quiz);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return quiz.Id;
    }
}