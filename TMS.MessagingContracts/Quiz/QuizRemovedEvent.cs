using TMS.Domain.Account;
using TMS.Domain.Quizzes;

namespace TMS.MessagingContracts.Quiz;

public record QuizRemovedEvent(QuizId QuizId,AccountId AccountId) ;