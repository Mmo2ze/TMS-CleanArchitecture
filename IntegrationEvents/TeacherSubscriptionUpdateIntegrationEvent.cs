using TMS.Domain.Common.Models;

namespace IntegrationEvents;

public record TeacherSubscriptionUpdateIntegrationEvent(
    Guid Id,
    DateOnly NewEOfSubscription,
    string TeacherPhone,
    string Name) : IntegrationEvent(Id);