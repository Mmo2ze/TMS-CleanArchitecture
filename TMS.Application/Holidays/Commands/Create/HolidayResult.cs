using TMS.Domain.Assistants;
using TMS.Domain.Groups;
using TMS.Domain.Holidays;

namespace TMS.Application.Holidays.Commands.Create;

public record HolidayResult(
    HolidayId Id,
    DateOnly StartDate,
    DateOnly EndDate,
    GroupId? GroupId,
    AssistantInfo CreatedBy,
    AssistantInfo? UpdatedBy);