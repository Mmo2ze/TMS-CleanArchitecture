using TMS.Domain.Assistants;

namespace TMS.Application.Quizzes.Queries.Get;

public record QuizAssistantResult(AssistantId Id, string Name)
{
    public static QuizAssistantResult From(Assistant assistant)
    {
        return new QuizAssistantResult(assistant.Id, assistant.Name);
    }
};