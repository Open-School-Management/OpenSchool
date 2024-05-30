using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceManager.Domain.Entities;
using ResourceManager.Domain.Enums;

namespace ResourceManager.Infrastructure.EntityConfigurations;

public class SharedConfiguration : IEntityTypeConfiguration<Shared>
{
    public void Configure(EntityTypeBuilder<Shared> builder)
    {
        
        builder.Property(e => e.PermissionType)
            .HasConversion(
                v => v.ToString(), 
                v => (PermissionType)System.Enum.Parse(typeof(PermissionType), v) 
            );
        
        builder.HasOne(s => s.Directory)
            .WithMany(d => d.Shareds)
            .HasForeignKey(f => f.DirectoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}