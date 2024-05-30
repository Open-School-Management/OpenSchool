using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceManager.Domain.Entities;
using Directory = ResourceManager.Domain.Entities.Directory;

namespace ResourceManager.Infrastructure.EntityConfigurations;

public class DirectoryConfiguration : PersonalizedEntityTypeConfiguration<Directory>
{
    public override void Configure(EntityTypeBuilder<Directory> builder)
    {
        base.Configure(builder);

        builder
            .Property(e => e.Name)
            .HasMaxLength(256);

        builder
            .Property(e => e.ParentId)
            .IsRequired(false);

        builder
            .Property(e => e.Path)
            .HasMaxLength(512);
        builder.Property(d => d.DuplicateNo)
            .IsRequired();

        builder.HasOne(d => d.Parent)
            .WithMany()
            .HasForeignKey(d => d.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.LockedDirectory)
            .WithOne(ld => ld.Directory)
            .HasForeignKey<LockedDirectory>(ld => ld.DirectoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Files)
            .WithOne(f => f.Directory)
            .HasForeignKey(f => f.DirectoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(d => d.Shareds)
            .WithOne(f => f.Directory)
            .HasForeignKey(f => f.DirectoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}