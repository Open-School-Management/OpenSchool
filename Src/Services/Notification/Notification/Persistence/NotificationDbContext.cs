using Microsoft.EntityFrameworkCore;
using SharedKernel.EFCore;

namespace Notification.Persistence;

public class NotificationDbContext : CoreDbContext
{
    public NotificationDbContext(DbContextOptions options) : base(options)
    {
        
    }
}