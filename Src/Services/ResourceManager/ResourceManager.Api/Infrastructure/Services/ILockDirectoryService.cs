namespace ResourceManager.Api.Infrastructure.Services;

public interface ILockDirectoryService
{
    Task MakeSureLockedDirectoryIsSafeAsync(long directoryId, string code = "", Action callback = default, CancellationToken cancellationToken = default);
}