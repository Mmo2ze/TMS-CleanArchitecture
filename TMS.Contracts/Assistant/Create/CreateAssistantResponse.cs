namespace TMS.Contracts.Assistant.Create;

public record CreateAssistantResponse(string Id, string Name, string Phone, string Email, IEnumerable<string> Roles);