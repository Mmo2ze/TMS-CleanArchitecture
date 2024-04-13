using TMS.Domain.Common.Models;

namespace TMS.Domain.Teachers.Events;

public record TeacherSubscriptionUpdateIntegrationEvent(
    Guid Id,
    DateOnly NewEOfSubscription,
    string TeacherPhone,
    string Name) : IntegrationEvent(Id);