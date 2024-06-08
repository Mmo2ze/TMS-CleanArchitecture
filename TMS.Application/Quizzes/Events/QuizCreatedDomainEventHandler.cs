using MassTransit;
using MediatR;
using TMS.Domain.Quizzes.Events;
using TMS.MessagingContracts.Quiz;

namespace TMS.Application.Quizzes.Events;

public class QuizCreatedDomainEventHandler: INotificationHandler<QuizCreatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public QuizCreatedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Handle(QuizCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _publishEndpoint.Publish(
            new QuizCreatedEvent(
                notification.QuizId,
                notification.Degree,
                notification.MaxDegree,
                notification.AccountId),
            cancellationToken);
        return Task.CompletedTask;
    }
}