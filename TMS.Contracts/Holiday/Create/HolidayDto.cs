using TMS.Domain.Assistants;
using TMS.Domain.Groups;
using TMS.Domain.Holidays;

namespace TMS.Contracts.Holiday.Create;

public record HolidayDto(
    string Id,
    DateOnly StartDate,
    DateOnly EndDate,
    string? GroupId,
    AssistantInfo CreatedBy,
    AssistantInfo? UpdatedBy);