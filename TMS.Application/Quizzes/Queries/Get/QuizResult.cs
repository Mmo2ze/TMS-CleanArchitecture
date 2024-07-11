using TMS.Domain.Assistants;
using TMS.Domain.Quizzes;

namespace TMS.Application.Quizzes.Queries.Get;

public record QuizResult(
    QuizId Id,
    double Degree,
    double MaxDegree,
    AssistantInfo? AddedBy,
    AssistantInfo? UpdatedBy,
    DateOnly CreatedAt,
    DateTime? UpdatedAt
)
{
    public static QuizResult From(Quiz quiz)
    {
        return new QuizResult(quiz.Id,
            quiz.Degree,
            quiz.MaxDegree,
            quiz.AddedBy == null ? null : new AssistantInfo(quiz.AddedBy.Name, quiz.AddedBy.Id),
            quiz.UpdatedBy == null ? null : new AssistantInfo(quiz.UpdatedBy.Name, quiz.UpdatedBy.Id),
            quiz.CreatedAt,
            quiz.UpdatedAt);
    }
};