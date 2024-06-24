using TMS.Domain.Accounts;
using TMS.Domain.Assistants;

namespace TMS.Domain.Quizzes.Events;

public record QuizCreatedDomainEvent(QuizId QuizId, double Degree, double MaxDegree, AccountId AccountId, AssistantId AddedBy) : DomainEvent;