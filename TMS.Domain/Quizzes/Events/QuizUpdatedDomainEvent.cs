using TMS.Domain.Assistants;

namespace TMS.Domain.Quizzes.Events;

public record QuizUpdatedDomainEvent(QuizId QuizId , double Degree, double MaxDegree, AssistantId UpdatedBy) : DomainEvent;