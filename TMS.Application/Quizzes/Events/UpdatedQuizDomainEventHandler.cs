using MassTransit;
using MediatR;
using TMS.Domain.Assistants;
using TMS.Domain.Quizzes.Events;
using TMS.MessagingContracts.Quiz;

namespace TMS.Application.Quizzes.Events;

public class UpdatedQuizDomainEventHandler : INotificationHandler<QuizUpdatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdatedQuizDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Handle(QuizUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _publishEndpoint.Publish(
            new QuizUpdatedEvent(
                notification.QuizId,
                notification.Degree,
                notification.MaxDegree,
                notification.AccountId,
                notification.UpdatedBy),
            cancellationToken);
        return Task.CompletedTask;
    }
}