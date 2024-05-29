using System.IO.Pipelines;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ResourceManager.CloudStorage.Models;

namespace ResourceManager.CloudStorage.Providers;

public class CloudflareStorageProvider : ICloudflareStorageProvider, IDisposable
{
    private readonly ILogger<CloudflareStorageProvider> _logger;
    private readonly AmazonS3Client _client;
    private readonly CloudflareSettings _cloudflareSettings;
    private const int EXPIRY_TIME = 365; // days;
    
    public CloudflareStorageProvider(IOptions<CloudflareSettings> options,  ILogger<CloudflareStorageProvider> logger)
    {
        _logger = logger;
        _cloudflareSettings = options.Value;
        _client = new AmazonS3Client(_cloudflareSettings.AccessKey, _cloudflareSettings.SecretKey, new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.APSoutheast1,
            RetryMode = RequestRetryMode.Standard,
            MaxErrorRetry = 3,
            ServiceURL = _cloudflareSettings.ServiceURL,
        });
    }
    
    public async Task<UploadResponse> UploadAsync(UploadRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentFileName = $"{Guid.NewGuid()}{model.FileExtension}";
            var request = new PutObjectRequest()
            {
                BucketName = _cloudflareSettings.BucketName,
                Key = string.IsNullOrEmpty(model.Prefix) ? $"{_cloudflareSettings.Root}/{currentFileName}" : $"{_cloudflareSettings.Root}/{model.Prefix}/{currentFileName}",
                InputStream = model.Stream,
            };
            
            await _client.PutObjectAsync(request, cancellationToken);
            return new UploadResponse()
            {
                OriginalFileName = model.FileName,
                CurrentFileName = currentFileName,
                FileExtension = model.FileExtension,
                Size = model.Size,
                Prefix = model.Prefix,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cloudflare R2 upload failed");
            return new UploadResponse()
            {
                Success = false,
                OriginalFileName = model.FileName,
                ErrorMessage = ex.Message,
            };
        }
    }

    public async Task<List<UploadResponse>> UploadAsync(List<UploadRequest> models, CancellationToken cancellationToken = default)
    {
        var responses = await Task.WhenAll(models.Select(model => this.UploadAsync(model, cancellationToken)));
        return responses.ToList();
    }

    public async Task<DownloadResponse> DownloadAsync(string fileName, string version = "", CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.GetObjectAsync(new GetObjectRequest()
            {
                BucketName = _cloudflareSettings.BucketName,
                Key = $"{_cloudflareSettings.Root}/{fileName}",
            }, cancellationToken);
        
            var publicUrl = _client.GetPreSignedURL(new GetPreSignedUrlRequest()
            {
                BucketName = _cloudflareSettings.BucketName,
                Key = $"{_cloudflareSettings.Root}/{fileName}",
                Expires = DateTime.UtcNow.AddDays(EXPIRY_TIME)
            });
        
            var ms = new MemoryStream();
            await response.ResponseStream.CopyToAsync(ms, cancellationToken);

            _logger.LogWarning("Download file from S3 with key = " + fileName);

            return new DownloadResponse()
            {
                PresignedUrl = publicUrl,
                ExpiryTime = EXPIRY_TIME,
                MemoryStream = ms,
                FileName = fileName,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"s3 download failed with specified key = {fileName}");
            throw;
        }
    }

    public async Task<List<DownloadResponse>> DownloadAsync(List<string> fileNames, string version = "", CancellationToken cancellationToken = default)
    {
        var tasks = fileNames.Select(f => this.DownloadAsync(f, cancellationToken: cancellationToken));
        return (await Task.WhenAll(tasks)).ToList();
    }

    public async Task DeleteAsync(string fileName, string version = "", CancellationToken cancellationToken = default)
    {
        var request = new DeleteObjectRequest()
        {
            BucketName = _cloudflareSettings.BucketName,
            Key = $"{_cloudflareSettings.Root}/{fileName}",
        };
        await _client.DeleteObjectAsync(request, cancellationToken);
    }

    public async Task<List<DownloadResponse>> DownloadDirectoryAsync(string directory, string version = "", CancellationToken cancellationToken = default)
    {
        var request = new ListObjectsV2Request()
        {
            BucketName = _cloudflareSettings.BucketName,
            Prefix = $"{_cloudflareSettings.Root}/{directory}"
        };

        var listObjects = await _client.ListObjectsV2Async(request, cancellationToken);
        var objects = listObjects.S3Objects.Where(o => !o.Key.Equals($"{_cloudflareSettings.Root}/{directory}/")).ToList();

        var tasks = objects.Select(s3 => Task.Run(() =>
        {
            var publicUrl = _client.GetPreSignedURL(new GetPreSignedUrlRequest()
            {
                BucketName = _cloudflareSettings.BucketName,
                Key = s3.Key,
                Expires = DateTime.UtcNow.AddDays(EXPIRY_TIME)
            });

            return new DownloadResponse()
            {
                PresignedUrl = publicUrl,
                ExpiryTime = EXPIRY_TIME,
                FileName = s3.Key
            };
        }));
        var response = await Task.WhenAll(tasks);

        return response.ToList();
    }

    public async Task<List<DownloadResponse>> DownloadPagingAsync(string directory, int pageIndex, int pageSize, string version = "", CancellationToken cancellationToken = default)
    {
        var request = new ListObjectsV2Request()
        {
            BucketName = _cloudflareSettings.BucketName,
            Prefix = $"{_cloudflareSettings.Root}/{directory}",
            MaxKeys = pageSize
        };

        var paginatorResponse = _client.Paginators.ListObjectsV2(request);
        await foreach (var paginator in paginatorResponse.Responses)
        {

            var objects = paginator.S3Objects.Where(o => !o.Key.Equals($"{_cloudflareSettings.Root}/{directory}/")).ToList();
            var tasks = objects.Select(s3 => Task.Run(() =>
            {
                var publicUrl = _client.GetPreSignedURL(new GetPreSignedUrlRequest()
                {
                    BucketName = _cloudflareSettings.BucketName,
                    Key = s3.Key,
                    Expires = DateTime.UtcNow.AddDays(EXPIRY_TIME)
                });

                return new DownloadResponse()
                {
                    PresignedUrl = publicUrl,
                    ExpiryTime = EXPIRY_TIME,
                };
            }));

            var response = await Task.WhenAll(tasks);
            return response.ToList();
        }
        return new List<DownloadResponse>();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}