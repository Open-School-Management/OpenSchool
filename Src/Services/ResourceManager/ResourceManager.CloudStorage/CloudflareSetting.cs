namespace ResourceManager.CloudStorage;

public class CloudflareSetting
{
    public string AccessKey { get; private set; }
    public string SecretKey { get; private set; }
    public string AccountId { get; private set; }
    public string BucketName { get; private set; }
    public string Root { get; private set; }
    public List<string> AcceptExtensions { get; private set; }
}