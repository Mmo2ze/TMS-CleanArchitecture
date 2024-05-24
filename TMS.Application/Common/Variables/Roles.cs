namespace TMS.Application.Common.Variables;

public static class Roles
{
    public const string CodeSent = "CodeSent";

    public static class Admin
    {
        public const string AdminNonRegister = "AdminNonRegister";
        public const string AdminCodeSent = "AdminCodeSent";
        public const string Role = "Admin";
    }
    public static class Student
    {
        public const string StudentNonRegister = "StudentNonRegister";
        public const string StudentCodeSent = "StudentCodeSent";
        public const string Role = "Student";
    }
    public static class Teacher
    {
        public const string TeacherNonRegister = "TeacherNonRegister";
        public const string Role = "Teacher";
        public const string TeacherCodeSent = "TeacherCodeSent";
        public const string Assistant = "Assistant";

    }
		
    public static class Parent
    {
        public const string ParentNonRegister = "ParentNonRegister";
        public  const string ParentCodeSent = "ParentCodeSent";
        public const string Role = "Parent";
    }
		
		
		
}