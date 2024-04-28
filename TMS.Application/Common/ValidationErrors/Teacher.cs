namespace TMS.Application.Common.ValidationErrors;

public static  class ValidationErrors
{
    public static class Teacher
    {
        public  static readonly ValidationError PhoneAlreadyExists = new("Teacher.PhoneAlreadyExists", "Phone number already exists");
        public static readonly ValidationError EmailAlreadyExists = new("Teacher.EmailAlreadyExists", "Email already exists");
    }
}