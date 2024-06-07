using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Quizzes.Commands.Delete;

public class DeleteQuizCommandHandler : IRequestHandler<DeleteQuizCommand, ErrorOr<string>>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuizCommandHandler(IQuizRepository quizRepository, IUnitOfWork unitOfWork,
        IAccountRepository accountRepository)
    {
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
    }

    public async Task<ErrorOr<string>> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
    {
        var account = _accountRepository.GetQueryable()
            .Include(x => x.Quizzes.Where(x => x.Id == request.Id))
            .First(x => x.Id == request.AccountId);

        account.RemoveQuiz(account.Quizzes[0]);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return string.Empty;
    }
}