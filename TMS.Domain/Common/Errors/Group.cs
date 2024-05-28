using ErrorOr;

namespace TMS.Domain.Common.Errors;

public static partial class Errors
{
    public static class Group
    {
        public static Error NotFound => Error.NotFound("Group.NotFound", "Group not found");
        public static Error GroupNameAlreadyExists => Error.Conflict("Group.NameAlreadyExists", "Group name already exists");
        
    }

    public static class Account
    {
        public static Error NotFound => Error.NotFound("Account.NotFound", "Account not found");
        public static Error AccountNameAlreadyExists => Error.Conflict("Account.NameAlreadyExists", "Account name already exists");
        
    }


}