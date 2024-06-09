using TMS.Domain.Parents;

namespace TMS.Application.Parents.Queries.Get;

public record ParentResult(
    ParentId Id,
    string Name,
    string? Email,
    string Phone);