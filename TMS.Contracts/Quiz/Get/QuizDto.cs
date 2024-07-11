namespace TMS.Contracts.Quiz.Get;

public record QuizDto(
    string Id,
    double Degree,
    double MaxDegree,
    QuizAssistantResponse ? AddedBy,
    QuizAssistantResponse? UpdatedBy,
    DateOnly CreatedAt,
    DateTime? UpdatedAt
    );