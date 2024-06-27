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

    public static class Student
    {
        public static Error NotFound => Error.NotFound("Student.NotFound", "Student not found");
        public static Error PhoneAlreadyExists => Error.Conflict("Student.PhoneAlreadyExists", "Phone already exists by other student");
        public static Error AlreadyInGroup => Error.Conflict("Student.AlreadyInGroup", "Student already in group");
    }

    public static class Holiday
    {
        public static Error NotFound => Error.NotFound("Holiday.NotFound", "Holiday not found");
        public static Error ConflictWithOtherHoliday => Error.Conflict("Holiday.ConflictWithOtherHoliday", "Holiday conflict with other holiday");
    }
}