using Microsoft.AspNetCore.Mvc;

namespace ResourceManager.Api.Apis.File;

public static class FilesApi
{
    public static RouteGroupBuilder MapFilesApiVersionOne(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/resources-manager/directories").HasApiVersion(1);

        api.MapGet("/all-by-directory/{directoryId}", CancelOrderAsync);
        api.MapGet("/{id}", CancelOrderAsync);
        api.MapGet("/paging", CancelOrderAsync);
        
        
        api.MapPost("/", CancelOrderAsync);
        api.MapPut("/", CancelOrderAsync);
        
        
        api.MapDelete("/", CancelOrderAsync);
        
        
        
        return api;
    }

    public static async Task CancelOrderAsync()
    {
        
    }
}