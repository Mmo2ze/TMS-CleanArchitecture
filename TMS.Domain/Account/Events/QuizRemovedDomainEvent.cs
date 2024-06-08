using TMS.Domain.Quizzes;

namespace TMS.Domain.Account.Events;

public record QuizRemovedDomainEvent(QuizId QuizId, AccountId accountId) : DomainEvent;