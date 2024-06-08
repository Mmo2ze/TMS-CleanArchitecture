using TMS.Domain.Quizzes;

namespace TMS.Application.Quizzes.Queries.Get;

public record QuizResult(
    QuizId Id,
    double Degree,
    double MaxDegree,
    QuizAssistantResult ? AddedBy,
    QuizAssistantResult? UpdatedBy,
    DateOnly CreatedAt,
    DateTime? UpdatedAt,
    string TeacherName)
{
    public static QuizResult From(Quiz quiz)
    {
        return new QuizResult(quiz.Id,
            quiz.Degree,
            quiz.MaxDegree,
            quiz.AddedBy == null ? null : QuizAssistantResult.From(quiz.AddedBy),
            quiz.UpdatedBy == null ? null : QuizAssistantResult.From(quiz.UpdatedBy),
            quiz.CreatedAt,
            quiz.UpdatedAt,
            quiz.Teacher.Name);
    }
};