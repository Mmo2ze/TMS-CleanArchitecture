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