using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceManager.Domain.Entities;

namespace ResourceManager.Infrastructure.EntityConfigurations;

public class ConfigConfiguration : PersonalizedEntityTypeConfiguration<Config>
{
    public override void Configure(EntityTypeBuilder<Config> builder)
    {
        base.Configure(builder);
    }
}