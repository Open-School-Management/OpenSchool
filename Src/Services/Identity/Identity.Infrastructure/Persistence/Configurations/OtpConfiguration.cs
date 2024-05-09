using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class OtpConfiguration : EntityAuditConfiguration<Otp>
{
    public override void Configure(EntityTypeBuilder<Otp> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(a => a.Id);
        
        builder
            .Property(a => a.Code)
            .HasMaxLength(50)
            .IsUnicode(false);
        
    }
}