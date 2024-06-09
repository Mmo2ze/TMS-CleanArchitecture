using TMS.Domain.Common.Enums;

namespace TMS.Contracts.Parent.Create;

public record CreateParentRequest(
    string Name,
    Gender Gender,
    string? Email = null,
    string? Phone = null);