using IntegrationEventLogs.Entities;

namespace IntegrationEventLogs.Services;

public interface IIntegrationEventLogService
{
    Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken = default);
    
    
    
}