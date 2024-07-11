using TMS.Domain.Assistants;

namespace TMS.Contracts.Holiday.Create;

public record HolidayDto(
    string Id,
    DateOnly StartDate,
    DateOnly EndDate,
    string? GroupId,
    AssistantInfo CreatedBy,
    AssistantInfo? UpdatedBy);