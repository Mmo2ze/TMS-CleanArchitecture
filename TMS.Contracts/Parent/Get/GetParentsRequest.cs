namespace TMS.Contracts.Parent.Get;

public record GetParentsRequest(
    int Page,
    int PageSize,
    string? Search,
    bool PhoneRequired = true) ;