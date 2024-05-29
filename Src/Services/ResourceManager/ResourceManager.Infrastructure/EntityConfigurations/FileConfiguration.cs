using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = ResourceManager.Domain.Entities.File;

namespace ResourceManager.Infrastructure.EntityConfigurations;

public class FileConfiguration : PersonalizedEntityTypeConfiguration<File>
{
    public override void Configure(EntityTypeBuilder<File> builder)
    {
        base.Configure(builder);
        
        builder
            .Property(f => f.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(f => f.OriginalFileName)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(f => f.FileExtension)
            .HasMaxLength(10);
        
        builder.HasOne(f => f.Directory)
            .WithMany(d => d.Files)
            .HasForeignKey(f => f.DirectoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}