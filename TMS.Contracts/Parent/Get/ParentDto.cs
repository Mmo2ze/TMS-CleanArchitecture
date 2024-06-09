namespace TMS.Contracts.Parent.Get;

public record ParentDto(
    string Id,
    string Name,
    string? Email,
    string Phone);