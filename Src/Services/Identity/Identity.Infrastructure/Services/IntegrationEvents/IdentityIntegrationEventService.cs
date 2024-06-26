using Identity.Application.IntegrationEvents.Services;
using Identity.Infrastructure.Persistence;
using IntegrationEventLogs;
using MessageBroker.Abstractions;
using MessageBroker.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Services.IntegrationEvents;

public class IdentityIntegrationEventService : IIdentityIntegrationEventService, IDisposable
{
    private volatile bool _disposedValue;

    private readonly IEventBus _eventBus;
    private readonly IdentityDbContext _context;
    private readonly IIntegrationEventLogService _integrationEventLogService;
    private readonly ILogger<IdentityIntegrationEventService> _logger;
    
    public IdentityIntegrationEventService(IEventBus eventBus,
        IdentityDbContext context,
        IIntegrationEventLogService integrationEventLogService,
        ILogger<IdentityIntegrationEventService> logger)
    {
        _eventBus = eventBus;
        _context = context; 
        _integrationEventLogService = integrationEventLogService;
        _logger = logger;
    }
    
    public async Task SaveEventAndIdentityContextChangesAsync(List<IntegrationEvent> events, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("IdentityIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", string.Join(",", events));

        //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
        await ResilientTransaction.New(_context).ExecuteAsync(async () =>
        {
            // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
            await _context.SaveChangesAsync(cancellationToken);
            await _integrationEventLogService.SaveEventsAsync(events, _context.Database.CurrentTransaction, cancellationToken);
        });
    }

    public async Task PublishThroughEventBusAsync(List<IntegrationEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var @event in events)
        {
           await PublishThroughEventBusAsync(@event, cancellationToken);
        }
    }
    
    public async Task PublishThroughEventBusAsync(IntegrationEvent evt, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Publishing integration event: {IntegrationEventId_published} - ({@IntegrationEvent})", evt.Id, evt);

            // await _integrationEventLogService.MarkEventAsInProgressAsync(evt.Id, cancellationToken);
            await _eventBus.PublishAsync(evt, cancellationToken);
            // await _integrationEventLogService.MarkEventAsPublishedAsync(evt.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", evt.Id, evt);
            // await _integrationEventLogService.MarkEventAsFailedAsync(evt.Id, cancellationToken);
        }
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                (_integrationEventLogService as IDisposable)?.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}