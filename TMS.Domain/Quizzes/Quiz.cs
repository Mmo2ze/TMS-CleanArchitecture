using TMS.Domain.Accounts;
using TMS.Domain.Assistants;
using TMS.Domain.Quizzes.Events;
using TMS.Domain.Teachers;

namespace TMS.Domain.Quizzes;

public class Quiz : Aggregate<QuizId>
{


    public double Degree { get; private set; }
    public double MaxDegree { get; private set; }
    public AccountId AccountId { get; private set; }
    public TeacherId TeacherId { get; private set; }
    public AssistantId AddedById { get; private set; }
    public AssistantId? UpdatedById { get; private set; }

    public Teacher Teacher { get; private set; }
    public Assistant? AddedBy { get; private set; }
    public Assistant? UpdatedBy { get; private set; }

    public DateOnly CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    
    public Quiz(QuizId id, double degree, double maxDegree, AccountId accountId, AssistantId?  addedById, TeacherId teacherId,DateOnly createdAt) : base(id)
    {
        Degree = degree;
        MaxDegree = maxDegree;
        AccountId = accountId;
        AddedById = addedById;
        TeacherId = teacherId;
        CreatedAt = createdAt;
    }

    public static Quiz Create(double degree, double maxDegree, AccountId accountId, AssistantId? addedBy, TeacherId teacherId,DateOnly  createdAt)
    {
        return new Quiz(QuizId.CreateUnique(), degree, maxDegree, accountId, addedBy,teacherId,createdAt);
    }

    public void Update(double degree, double maxDegree, AssistantId? updatedBy)
    {
        Degree = degree;
        MaxDegree = maxDegree;
        UpdatedById = updatedBy;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new QuizUpdatedDomainEvent(Id, degree, maxDegree,AccountId, updatedBy));
    }
}