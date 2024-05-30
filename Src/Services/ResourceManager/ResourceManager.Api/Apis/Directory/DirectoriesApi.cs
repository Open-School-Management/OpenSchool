using Asp.Versioning.Builder;

namespace ResourceManager.Api.Apis.Directory;

public static class DirectoriesApi
{
    private const string BASE_URL = "/api/v{version:apiVersion}/resources-manager/directories";
    
    public static IVersionedEndpointRouteBuilder MapDirectoriesApiVersionOne(this IVersionedEndpointRouteBuilder builder)
    {
        var groups = builder.MapGroup(BASE_URL).HasApiVersion(1);

        groups.MapGet("/", () => "Hello world");
        
        return builder;
    }
    
}