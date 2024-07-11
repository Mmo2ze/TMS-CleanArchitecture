using TMS.Domain.Accounts;
using TMS.Domain.Quizzes;

namespace TMS.MessagingContracts.Quiz;

public record QuizCreatedEvent(QuizId QuizId, double Degree, double MaxDegree, AccountId AccountId) ;