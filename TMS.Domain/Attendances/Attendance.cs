using TMS.Domain.Accounts;
using TMS.Domain.Assistants;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Domain.Attendances;

public class Attendance : Aggregate<AttendanceId>
{
    public AttendanceStatus Status { get; private set; }
    public AccountId AccountId { get; private set; }
    public TeacherId TeacherId { get; private set; }
    public AssistantId? CreatedById { get; private set; }
    public AssistantId? UpdatedById { get; private set; }
    public DateOnly Date { get; private set; }
    public DateTime ?UpdatedAt { get; private set; }
    public Teacher Teacher { get; private set; }
    
    public Account Account { get; private set; }
    public Assistant? CreatedBy { get; private set; }
    public Assistant? UpdatedBy { get; private set; }
    
}