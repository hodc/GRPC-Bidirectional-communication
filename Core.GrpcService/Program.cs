using Core.ProtosLibrary;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

builder.Services.AddTransient<IFileManager, FileManager>();
builder.Services.AddSingleton<ICommanderManager, CommanderManager>();
builder.Services.AddSingleton<IConfiguration>(config);
// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

var commanderManager = app.Services.GetRequiredService<ICommanderManager>();
// Configure the HTTP request pipeline.
app.MapGrpcService<FilesActionsService>();
app.MapGrpcService<CommandsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


app.Run();
