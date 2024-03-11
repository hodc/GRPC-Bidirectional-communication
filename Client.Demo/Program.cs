using Grpc.Net.Client;
using Core.ProtosLibrary;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;
using System.Collections.Concurrent;

var _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Program>();
var _loggerClientfileActions = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ClientFilesActions>();
var _commandQueueRequest = new ConcurrentQueue<CommandRequest>();
var _commandQueueResponse = new ConcurrentQueue<CommandResponse>();
IClientFilesActions _clientFilesActions = new ClientFilesActions();
IManagerActions _managerActions = new ManagerActions();


try
{
    var ServerAddress = "https://localhost:7200";
    var httpHandler = new HttpClientHandler();
    httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    var clientId = "aaaa-aaaa-aaaa-aaaa";

    while (true)
    {
        try
        {
            await _managerActions.Run(ServerAddress, clientId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
