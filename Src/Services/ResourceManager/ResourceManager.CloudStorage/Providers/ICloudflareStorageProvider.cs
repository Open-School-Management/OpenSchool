﻿using ResourceManager.CloudStorage.Models;

namespace ResourceManager.CloudStorage.Providers;

public interface ICloudflareStorageProvider : IStorageProvider
{
    /// <summary>
    /// Trả về tất cả file trong directory
    /// </summary>
    Task<List<DownloadResponse>> DownloadDirectoryAsync(string directory, string version = "", CancellationToken cancellationToken = default);

    /// <summary>
    /// Trả về file paging trong directory
    /// </summary>
    Task<List<DownloadResponse>> DownloadPagingAsync(string directory, int pageIndex, int pageSize, string version = "", CancellationToken cancellationToken = default);
}