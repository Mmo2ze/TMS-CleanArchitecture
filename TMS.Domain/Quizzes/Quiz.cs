using ErrorOr;
using TMS.Domain.Account;
using TMS.Domain.Assistants;
using TMS.Domain.Teachers;

namespace TMS.Domain.Quizzes;

public class Quiz : Aggregate<QuizId>
{
    public double Degree { get; private set; }
    public double MaxDegree { get; private set; }
    public AccountId AccountId { get; private set; }

    public AssistantId AddedById { get; private set; }
    public Assistant AddBy { get; private set; }
    public AssistantId? UpdatedById { get; private set; }
    public Assistant? UpdatedBy { get; private set; }
    public TeacherId TeacherId { get; private set; }
    public Teacher Teacher { get; private set; }

    public Quiz(QuizId id, double degree, double maxDegree, AccountId accountId, AssistantId addedBy) : base(id)
    {
        Degree = degree;
        MaxDegree = maxDegree;
        AccountId = accountId;
        AddedById = addedBy;
    }

    public Quiz Create(double degree, double maxDegree, AccountId accountId, AssistantId addedBy)
    {
        RaiseDomainEvent(new QuizCreatedDomainEvent(Id, degree, maxDegree, accountId, addedBy));
        return new Quiz(QuizId.CreateUnique(), degree, maxDegree, accountId, addedBy);
    }

    public void Update(double degree, double maxDegree, AssistantId updatedBy)
    {
        Degree = degree;
        MaxDegree = maxDegree;
        UpdatedById = updatedBy;
        RaiseDomainEvent(new QuizUpdatedDomainEvent(Id, degree, maxDegree, updatedBy));
    }
}