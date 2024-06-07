using TMS.Domain.Account;
using TMS.Domain.Assistants;

namespace TMS.Domain.Quizzes;

public record QuizCreatedDomainEvent(QuizId QuizId, double Degree, double MaxDegree, AccountId AccountId, AssistantId AddedBy) : DomainEvent;