using ResourceManager.Api.Apis.Directory;
using ResourceManager.Api.Apis.File;
using ResourceManager.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

var withApiVersioning = builder.Services.AddApiVersioning();

builder.Services.AddDefaultOpenApi(withApiVersioning);

var app = builder.Build();

app.UseHttpsRedirection();

app.NewVersionedApi("Directories")
    .MapDirectoriesApiVersionOne();

app.NewVersionedApi("Files")
    .MapFilesApiVersionOne();

app.UseDefaultOpenApi();

app.Run();
