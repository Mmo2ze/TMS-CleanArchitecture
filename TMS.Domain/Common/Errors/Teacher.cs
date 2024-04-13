using ErrorOr;

namespace TMS.Domain.Common.Errors;

public static partial class Errors
{
    public static class Teacher
    {
        public static Error TeacherNotFound => Error.NotFound("Teacher.TeacherNotFound", "Teacher not found");
        public static Error PhoneAlreadyExists => Error.Conflict("Teacher.PhoneAlreadyExists", "Phone already exists by other teacher");
        public static Error EmailAlreadyExists => Error.Conflict("Teacher.EmailAlreadyExists", "Email already exists by other teacher");
    }
}