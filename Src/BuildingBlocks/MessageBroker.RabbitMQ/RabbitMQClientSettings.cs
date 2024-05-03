namespace MessageBroker.RabbitMQ;

public sealed class RabbitMQClientSettings
{
    /// <summary>
    /// Gets or sets the connection string of the RabbitMQ server to connect to.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// <para>Gets or sets the maximum number of connection retry attempts.</para>
    /// <para>Default value is 5, set it to 0 to disable the retry mechanism.</para>
    /// </summary>
    public int MaxConnectRetryCount { get; set; } = 5;
    
    public int RetryCount { get; set; } = 10;
    
    public string SubscriptionClientName { get; set; }
    
}
