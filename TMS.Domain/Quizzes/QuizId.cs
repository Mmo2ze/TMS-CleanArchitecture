namespace TMS.Domain.Quizzes;

public record QuizId(string Value) : ValueObjectId<QuizId>(Value)
{
    public QuizId() : this(Ulid.NewUlid().ToString())
    {
    }
}