using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceManager.Domain.Entities;
using ResourceManager.Domain.Enums;

namespace ResourceManager.Infrastructure.EntityConfigurations;

public class RecycleBinConfiguration : PersonalizedEntityTypeConfiguration<RecycleBin>
{
    public override void Configure(EntityTypeBuilder<RecycleBin> builder)
    {
        builder.Property(rb => rb.ResourceId)
            .IsRequired();

        builder.Property(rb => rb.ResourceType)
            .IsRequired();
        
        builder.Property(rb => rb.ResourceType)
            .HasConversion(
                v => v.ToString(), 
                v => (ResourceType)System.Enum.Parse(typeof(ResourceType), v) 
            );

        builder.Property(rb => rb.RestoredAt)
            .IsRequired();
    }
}