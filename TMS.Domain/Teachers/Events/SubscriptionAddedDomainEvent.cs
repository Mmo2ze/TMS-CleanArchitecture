namespace TMS.Domain.Teachers;

public record SubscriptionAddedDomainEvent(
    Guid Id,
    TeacherId TeacherId,
    string Name,
    string Phone,
    int Days,
    DateOnly EndOfSubscription) : DomainEvent(Id)
{
}