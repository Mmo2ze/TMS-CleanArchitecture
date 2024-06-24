using TMS.Domain.Accounts;
using TMS.Domain.Assistants;
using TMS.Domain.Quizzes;

namespace TMS.MessagingContracts.Quiz;

public record QuizUpdatedEvent(QuizId QuizId, double Degree, double MaxDegree, AccountId AccountId, AssistantId UpdatedBy);