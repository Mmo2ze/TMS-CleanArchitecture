namespace TMS.Contracts.Assistant.Update;

public record UpdateAssistantRequest(string Id,string Phone, string Name, string? Email, List<string> Roles);