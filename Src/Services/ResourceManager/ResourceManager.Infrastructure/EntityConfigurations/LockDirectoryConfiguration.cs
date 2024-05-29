using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceManager.Domain.Entities;

namespace ResourceManager.Infrastructure.EntityConfigurations;

public class LockedDirectoryConfiguration : PersonalizedEntityTypeConfiguration<LockedDirectory>
{
    public override void Configure(EntityTypeBuilder<LockedDirectory> builder)
    {

        builder.Property(ld => ld.Password)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasOne(ld => ld.Directory)
            .WithOne(d => d.LockedDirectory)
            .HasForeignKey<LockedDirectory>(ld => ld.DirectoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}