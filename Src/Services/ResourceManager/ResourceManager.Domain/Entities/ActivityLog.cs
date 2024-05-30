using System.ComponentModel.DataAnnotations.Schema;
using ResourceManager.Domain.Enums;
using SharedKernel.Domain;

namespace ResourceManager.Domain.Entities;

[Table("activity_log")]
public class ActivityLog : EntityBase
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ActionType ActionType { get; set; }
    public Guid ResourceId { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public Guid? SourceId { get; set; } 
    public Guid? DestinationId { get; set; } 
    public DateTime Timestamp { get; set; }
}