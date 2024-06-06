using TMS.Domain.Assistants;

namespace TMS.Application.Assistants.Commands.Create;

public record AssistantDto(AssistantId Id, string Name, string Phone, string? Email, IEnumerable<AssistantRole> Roles)
{
    public static AssistantDto From(Assistant assistant)
    {
        return new AssistantDto(assistant.Id, assistant.Name, assistant.Phone, assistant.Email, assistant.Roles);
    }
};