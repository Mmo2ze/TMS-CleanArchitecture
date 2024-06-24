using TMS.Domain.Quizzes;

namespace TMS.Domain.Accounts.Events;

public record QuizRemovedDomainEvent(QuizId QuizId, AccountId accountId) : DomainEvent;