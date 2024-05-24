namespace TMS.Application.Common.ValidationErrors;

public static partial class ValidationErrors
{
    public static class Group
    {
        public static readonly ValidationError NameAlreadyExists = new("Group.NameAlreadyExists", "Group name already exists");
        public static readonly ValidationError NotFound = new("Group.NotFound", "Group not found");
        
    }

    public static class Student
    {
        public static readonly ValidationError NotFound = new("Student.NotFound", "Student not found");

    }

    public static class Session
    {
        public static readonly ValidationError SessionIsConflict = new("Session.SessionIsConflict", "this session is conflict with another session");
    }
}