using TMS.Domain.Assistants;
using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Domain.Holidays;

public class Holiday : Aggregate<HolidayId>
{
    public TeacherId TeacherId { get; private set; }
    public GroupId? GroupId { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public AssistantId? CreatedById { get; private set; }
    public Assistant? CreatedBy { get; private set; }
    public AssistantId? ModifiedById { get; private set; }
    public Assistant? ModifiedBy { get; private set; }


    private Holiday(HolidayId id, TeacherId teacherId, GroupId? groupId, DateOnly startDate, DateOnly endDate,
        AssistantId? createdById) : base(id)
    {
        Id = id;
        TeacherId = teacherId;
        GroupId = groupId;
        StartDate = startDate;
        EndDate = endDate;
        CreatedById = createdById;
    }

    public static Holiday Create(TeacherId teacherId, GroupId? groupId, DateOnly startDate, DateOnly endDate,
        AssistantId? createdById)
    {
        return new Holiday(new HolidayId(), teacherId, groupId, startDate, endDate, createdById);
    }

    public void Update(DateOnly requestStartDate, DateOnly requestEndDate, GroupId? requestGroupId,AssistantId? modifiedById)
    {
        StartDate = requestStartDate;
        EndDate = requestEndDate;
        GroupId = requestGroupId;
        ModifiedById = modifiedById;
    }
    
}