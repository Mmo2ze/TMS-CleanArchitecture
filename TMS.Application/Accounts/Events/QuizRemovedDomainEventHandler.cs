using MediatR;
using TMS.Domain.Account;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Accounts.Events;

public class QuizRemovedDomainEventHandler: INotificationHandler<QuizRemovedDomainEvent>
{
    private readonly IQuizRepository _quizRepository;

    public QuizRemovedDomainEventHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task Handle(QuizRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetAsync(notification.QuizId, cancellationToken);
        _quizRepository.Remove(quiz!);
        
    }
}