using TMS.Domain.Accounts;
using TMS.Domain.Quizzes;

namespace TMS.MessagingContracts.Quiz;

public record QuizRemovedEvent(QuizId QuizId,AccountId AccountId) ;