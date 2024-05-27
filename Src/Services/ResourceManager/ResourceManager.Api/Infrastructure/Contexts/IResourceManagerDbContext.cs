namespace ResourceManager.Api.Infrastructure.Contexts;

public interface IResourceManagerDbContext : IBaseDbContext
{
    DbSet<ActivityLog> ActivityLogs { get; set; }
    DbSet<Config> Configs { get; set; }
    DbSet<Directory> Directories { get; set; }
    DbSet<File> Files { get; set; }
    DbSet<LockedDirectory> LockedDirectories { get; set; }
    DbSet<RecycleBin> RecycleBins { get; set; }
}