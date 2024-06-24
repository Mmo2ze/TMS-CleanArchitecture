using MassTransit;
using MediatR;
using TMS.Domain.Accounts.Events;
using TMS.Domain.Common.Repositories;
using TMS.MessagingContracts.Quiz;

namespace TMS.Application.Accounts.Events;

public class QuizRemovedDomainEventHandler: INotificationHandler<QuizRemovedDomainEvent>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public QuizRemovedDomainEventHandler(IQuizRepository quizRepository, IPublishEndpoint publishEndpoint)
    {
        _quizRepository = quizRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(QuizRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetAsync(notification.QuizId, cancellationToken);
        _quizRepository.Remove(quiz!);
        _publishEndpoint.Publish(new QuizRemovedEvent(notification.QuizId,notification.accountId), cancellationToken);
    }
}