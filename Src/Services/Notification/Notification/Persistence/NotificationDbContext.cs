using Microsoft.EntityFrameworkCore;
using SharedKernel.EntityFrameworkCore.DbContext;

namespace Notification.Persistence;

public class NotificationDbContext : BaseDbContext
{
    public NotificationDbContext(DbContextOptions options) : base(options)
    {
        
    }
}