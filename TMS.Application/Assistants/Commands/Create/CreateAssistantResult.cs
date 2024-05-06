using TMS.Domain.Assistants;

namespace TMS.Application.Assistants.Commands.Create;

public record CreateAssistantResult(AssistantId Id, string Name, string Phone)
{
    public static CreateAssistantResult From(Assistant assistant)
    {
        return new CreateAssistantResult(assistant.Id, assistant.Name, assistant.Phone);
    }
};