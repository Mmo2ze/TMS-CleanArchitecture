using TMS.Domain.Assistants;

namespace TMS.Contracts.Assistant.Create;

public record CreateAssistantRequest(string Phone, string Name, string? Email,List<AssistantRole> Roles);