using TMS.Domain.Common.Enums;

namespace TMS.Domain.Assistants;

public enum AssistantRole
{
    AddStudent,
    RemoveStudent,
    AddSession,
    RemoveSession,
    RegisterPayment,
    ViewPayments,
    RecordAttendance
}

public static class AssistantRoleString
{
    public const string AddStudent = "AddStudent";
    public const string RemoveStudent = "RemoveStudent";
    public const string AddSession = "AddSession";
    public const string RemoveSession = "RemoveSession";
    public const string RegisterPayment = "RegisterPayment";
    public const string ViewPayments = "ViewPayments";
    public const string RecordAttendance = "RecordAttendance";
}
public static class AssistantRoleExtenion
{
    public static Role ToBasic(this AssistantRole assistantRole)
    {
        return assistantRole switch
        {
            AssistantRole.AddStudent => Role.AddStudent,
            AssistantRole.RemoveStudent => Role.RemoveStudent,
            AssistantRole.AddSession => Role.AddSession,
            AssistantRole.RemoveSession => Role.RemoveSession,
            AssistantRole.RegisterPayment => Role.RegisterPayment,
            AssistantRole.ViewPayments => Role.ViewPayments,
            AssistantRole.RecordAttendance => Role.RecordAttendance,
            _ => Role.Assistant
        };
    }
    public static List<Role> ToBasicRoleList(this List<AssistantRole> assistantRoles)
    {
        return assistantRoles.Select(role => role.ToBasic()).ToList();
    }
}