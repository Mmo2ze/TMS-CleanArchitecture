using TMS.Domain.Common.Enums;

namespace TMS.Domain.Assistants;

public enum AssistantRole
{
    AddStudent,
    RemoveStudent,
    AddGroup,
    RecordPayment,
    ViewPayments,
    RecordAttendance,
    AddCardOrder,
    AddQuiz,
    AddHoliday,
    ScheduleAttendance
    
}


public static class AssistantRoleExtension
{
    public static Role ToBasic(this AssistantRole assistantRole)
    {
        return assistantRole switch
        {
            AssistantRole.AddStudent => Role.AddStudent,
            AssistantRole.RemoveStudent => Role.RemoveStudent,
            AssistantRole.AddGroup => Role.AddGroup,
            AssistantRole.RecordPayment => Role.RecordPayment,
            AssistantRole.ViewPayments => Role.ViewPayments,
            AssistantRole.RecordAttendance => Role.RecordAttendance,
            AssistantRole.AddCardOrder => Role.AddCardOrder,
            AssistantRole.AddQuiz => Role.AddQuiz,
            AssistantRole.AddHoliday => Role.AddHoliday,
            AssistantRole.ScheduleAttendance => Role.ScheduleAttendance,
            _ => Role.Assistant
        };
    }
    public static List<Role> ToBasicRoleList(this List<AssistantRole> assistantRoles)
    {
        return assistantRoles.Select(role => role.ToBasic()).ToList();
    }
}