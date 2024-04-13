using TMS.Domain.Common.Models;

namespace IntegrationEvents;

public record TeacherCreatedIntegrationEvent(
    Guid Id,
    string TeacherPhone,
    string TeacherName,
    DateOnly EndOfSubscription) : IntegrationEvent(Id);