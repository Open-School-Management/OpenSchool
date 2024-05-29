using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceManager.Domain.Entities;
using ResourceManager.Domain.Enums;

namespace ResourceManager.Infrastructure.EntityConfigurations;

public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder
            .Property(e => e.SourceId)
            .IsRequired(false);
        
        builder
            .Property(e => e.DestinationId)
            .IsRequired(false);
        
        builder.Property(rb => rb.ActionType)
            .HasConversion(
                v => v.ToString(), 
                v => (ActionType)System.Enum.Parse(typeof(ActionType), v) 
            );
    }
}