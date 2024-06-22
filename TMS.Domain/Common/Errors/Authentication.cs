using ErrorOr;

namespace TMS.Domain.Common.Errors;

public static partial class Errors
{
    public static class Auth
    {
        public static Error InvalidCredentials => Error.Unauthorized("Auth.InvalidCredentials", "Invalid credentials");
        public static Error InvalidCode => Error.Unauthorized("Auth.InvalidCode", "Invalid code");

        public static Error CodeExpired(DateTime date) =>
            Error.Unauthorized("Auth.CodeExpired", $"Code expired at {date}");

        public static Error UnauthorizedToBeAdmin =>
            Error.Unauthorized("Auth.UnauthorizedTobeAdmin", "You are not authorized to be an admin");

        public static Error ToManyTry => Error.Unauthorized("Auth.ToManyTry", "You have tried too many times");
        public static Error PhoneAlreadyExits => Error.Conflict("Auth.PhoneAlreadyExits", "Phone already exists");
        public static Error EmailAlreadyExists => Error.Conflict("Auth.EmailAlreadyExists", "Email already exists");
        public static Error YouAreNotAdmin => Error.Unauthorized("Auth.YouAreNotAdmin", "You are not an admin");

        public static Error NotTeacherOrAssistant =>
            Error.Unauthorized("Auth.NotTeacherOrAssistant", "You are not a teacher or assistant");
    }


    public static class Quiz
    {
        public static Error NotFound => Error.NotFound("Quiz.NotFound", "Quiz not found");
    }

    public static class Parnet
    {
        public static Error NotFound => Error.NotFound("Parent.NotFound", "Parent not found");
        public static Error  PhoneAlreadyExists => Error.Conflict("Parent.PhoneAlreadyExists", "Phone already exists by other parent");
    }
}