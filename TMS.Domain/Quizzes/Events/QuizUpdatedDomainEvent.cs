using TMS.Domain.Assistants;

namespace TMS.Domain.Quizzes;

public record QuizUpdatedDomainEvent(QuizId QuizId , double Degree, double MaxDegree, AssistantId UpdatedBy) : DomainEvent;