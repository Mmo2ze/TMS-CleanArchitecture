namespace TMS.Application.Common.ValidationErrors;

public static partial class ValidationErrors
{
    public static class Group
    {
        public static readonly ValidationError NameAlreadyExists = new("Group.NameAlreadyExists", "Group name already exists");
        public static readonly ValidationError NotFound = new("Group.NotFound", "Group not found");
        
    }
}