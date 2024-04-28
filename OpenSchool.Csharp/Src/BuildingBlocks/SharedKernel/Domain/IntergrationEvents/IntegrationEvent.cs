namespace SharedKernel.Domain;

public class IntegrationEvent
{
    public Guid Id { get; protected set; }

    public DateTime Timestamp { get; protected set; }
    public object Body { get; protected set; }

    public IntegrationEvent(Guid id, DateTime timestamp, object body)
    {
        Id = id;
        Timestamp = timestamp;
        Body = body;
    }
}

