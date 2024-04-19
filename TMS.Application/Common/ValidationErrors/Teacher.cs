namespace TMS.Application.Common.ValidationErrors;

public static partial class ValidationErrors
{
    public static class Teacher
    {
        public  static ValidationError PhoneAlreadyExists = new("Teacher.PhoneAlreadyExists", "Phone number already exists");
        public static ValidationError EmailAlreadyExists = new("Teacher.EmailAlreadyExists", "Email already exists");
    }
}