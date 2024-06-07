using TMS.Domain.Quizzes;

namespace TMS.Domain.Account;

public record QuizRemovedDomainEvent(QuizId QuizId, AccountId accountId) : DomainEvent;