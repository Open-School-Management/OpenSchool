using SharedKernel.Auth;
using SharedKernel.Libraries;

namespace SharedKernel.Domain;

public class AuditEvent : DomainEvent
{
    public AuditEvent(string tableName, ICurrentUser currentUser, Guid eventId = default) : base(eventId, null, currentUser)
    {
        if (eventId == Guid.Empty)
        {
            EventId = Guid.NewGuid();
        }
        
       
        else
        {
            IpAddress = "";
        }
    }
    
    public string IpAddress { get; protected set; }
}