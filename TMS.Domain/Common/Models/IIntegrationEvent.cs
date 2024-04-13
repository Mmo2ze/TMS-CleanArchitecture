using MediatR;

namespace TMS.Domain.Common.Models;

/**
 * This is IntegrationEvent Rolls
 * Any Aggregate Root should implement this interface
 * IntegrationEvents are used to communicate with outside world
 * If IntegrationEvent Failed to be processed, the Aggregate shouldn't be rolled back
 **/
public record IntegrationEvent(Guid Id):INotification;