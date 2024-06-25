using Microsoft.EntityFrameworkCore;
using TMS.Domain.Accounts;
using TMS.Domain.Assistants;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Domain.Attendances;

public class Attendance : Aggregate<AttendanceId>
{
    private Attendance(AttendanceId id, AccountId accountId, TeacherId teacherId, AssistantId? createdById,
        DateOnly date, AttendanceStatus status)
    {
        Id = id;
        AccountId = accountId;
        TeacherId = teacherId;
        CreatedById = createdById;
        Date = date;
        Status = status;
    }

    public AttendanceStatus Status { get; private set; }
    public AccountId AccountId { get; private set; }
    public TeacherId TeacherId { get; private set; }
    public AssistantId? CreatedById { get; private set; }
    public AssistantId? UpdatedById { get; private set; }
    public DateOnly Date { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Teacher Teacher { get; private set; } = null!;

    public Account Account { get; private set; } = null!;
    public Assistant? CreatedBy { get; private set; }
    public Assistant? UpdatedBy { get; private set; }

    public static Attendance Create(AccountId accountId, TeacherId teacherId, AssistantId? createdById, DateOnly date,
        AttendanceStatus status)
    {
        return new Attendance(AttendanceId.CreateUnique(), accountId, teacherId, createdById, date, status);
    }
    
    public void Update(AssistantId? updatedById, AttendanceStatus status)
    {
        UpdatedById = updatedById;
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}