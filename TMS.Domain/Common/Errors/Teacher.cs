using ErrorOr;

namespace TMS.Domain.Common.Errors;

public static partial class Errors
{
    public static class Teacher
    {
        public static Error TeacherNotFound => Error.NotFound("Teacher.TeacherNotFound", "Teacher not found");

        public static Error PhoneAlreadyExists =>
            Error.Conflict("Teacher.PhoneAlreadyExists", "Phone already exists by other teacher");

        public static Error EmailAlreadyExists =>
            Error.Conflict("Teacher.EmailAlreadyExists", "Email already exists by other teacher");
    }

    public static class Student
    {
        public static Error NotFound => Error.NotFound("Student.NotFound", "Student not found");

        public static Error PhoneAlreadyExists =>
            Error.Conflict("Student.PhoneAlreadyExists", "Phone already exists by other student");

        public static Error AlreadyInGroup => Error.Conflict("Student.AlreadyInGroup", "Student already in group");
    }

    public static class Holiday
    {
        public static Error NotFound => Error.NotFound("Holiday.NotFound", "Holiday not found");

        public static Error ConflictWithOtherHoliday => Error.Conflict("Holiday.ConflictWithOtherHoliday",
            "Holiday conflict with other holiday");
    }

    public static class Payment
    {
        public static Error BillDateAlreadyExists =>
            Error.Conflict("Payment.BillDateAlreadyExists", "Bill date already exists");

        public static Error NotFound => Error.NotFound("Payment.NotFound", "Payment not found");

        public static Error AccountHasBeenDeleted =>
            Error.NotFound("Payment.AccountHasBeenDeleted", "Account has been deleted");
    }

    public static class CardOrder
    {
        public static Error NotFound => Error.NotFound("Card.NotFound", "Card not found");

        public static Error AccountsNotFound => Error.NotFound("CardOrder.AccountsNotFound", "Accounts not found");
        public static Error OrderAlreadyAccepted => Error.Conflict("CardOrder.OrderAlreadyAccepted", "Order already accepted");
        public static Error OrderAlreadyRejected => Error.Conflict("CardOrder.OrderAlreadyRejected", "Order already rejected");
        public static Error OrderIsNotOnProcessing => Error.Conflict("CardOrder.OrderIsNotOnProcessing", "Order is not on processing");
        public static Error CanNotCompletePendingOrder => Error.Conflict("CardOrder.CanNotCompletePendingOrder", "Can not complete pending order");
        public static Error OrderIsNotOnPending => Error.Conflict("CardOrder.OrderIsNotOnPending", "Order is not on pending");
    }
}