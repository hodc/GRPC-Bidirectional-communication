using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.ProtosLibrary;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Configuration;

public class CommandsService : CommandService.CommandServiceBase
{
    private readonly ILogger<CommandsService> _logger;
    private readonly ConcurrentDictionary<string, IServerStreamWriter<CommandRequest>> _activeClients = new ConcurrentDictionary<string, IServerStreamWriter<CommandRequest>>();
    private static Dictionary<string, List<CommandRequest>> _pendingCommands = new Dictionary<string, List<CommandRequest>>();
    private static object lockObject = new object();
    private readonly IFileManager _fileManager;
    // constructor
    public CommandsService(ILogger<CommandsService> logger, IFileManager fileManager)
    {
        Console.WriteLine("CommandsService initialized");
        _logger = logger;
        _fileManager = fileManager;
        // _activeClients 

        // _pendingCommands = new Dictionary<string, List<CommandRequest>>();
        // _pendingCommands.TryAdd("aaaa-aaaa-aaaa-aaaa",
        //     new List<CommandRequest>(){
        //         new CommandRequest
        //         {
        //             RequestId =Guid.NewGuid().ToString(),
        //             ExecuteProcessCommand = new ExecuteProcessCommand
        //             {
        //                 ProcessName = "test",
        //                 ProcessArguments = { "arg1", "arg2" }
        //             }
        //         },
        //         new CommandRequest{
        //             RequestId = Guid.NewGuid().ToString(),
        //             ReadFileCommand = new ReadFileCommand
        //             {
        //                 FilePath = "test.txt"
        //             }
        //         }
        //     });
    }


    public override async Task SubscribeForCommands(Empty request, IServerStreamWriter<CommandRequest> responseStream, ServerCallContext context)
    {
        var clientId = context.RequestHeaders.Get("client-id")!.Value ?? "";
        var osVersion = context.RequestHeaders.Get("os-version")!.Value ?? "";
        Console.WriteLine($"SubscribeForCommands: Client connected: {clientId}, OS version: {osVersion}");
        try
        {
            // Placeholder logic to send commands to the client
            while (!context.CancellationToken.IsCancellationRequested)
            {
                if (_pendingCommands.ContainsKey(clientId) && _pendingCommands[clientId].Count > 0)
                {
                    _logger.LogInformation($"Sending {clientId} commands");
                    foreach (var command in _pendingCommands[clientId])
                    {
                        _logger.LogInformation($"Sending command:{command.CommandCase.ToString()} to client: {clientId}, RequestId: {command.RequestId}");
                        await responseStream.WriteAsync(command);
                    }
                    lock (lockObject)
                    {
                        _pendingCommands[clientId].Clear();
                    }
                }
                await Task.Delay(2000); // Adjust the interval based on your needs
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
            // Handle cancellation exception
            _logger.LogInformation("Client canceled the bidirectional stream.");
        }
        catch (Exception ex)
        {
            // Handle exceptions related to gRPC communication
            _logger.LogError(ex, "An error occurred while processing the bidirectional stream.");
        }
        finally
        {
            _activeClients.TryRemove(clientId, out var _);
        }
    }

    public override async Task StreamCommands(IAsyncStreamReader<CommandResponse> requestStream, IServerStreamWriter<CommandResponseStatus> responseStream, ServerCallContext context)
    {
        string clientId = "";

        try
        {
            clientId = context.RequestHeaders.Get("client-id")!.Value ?? "";
            var osVersion = context.RequestHeaders.Get("os-version")!.Value ?? "";
            Console.WriteLine($"StreamCommands: Client connected: {clientId}, OS version: {osVersion}");

            await foreach (var request in requestStream.ReadAllAsync())
            {
                Console.WriteLine($"Received command from client: {clientId}");
                // Process the received command and send a response
                var retVal = await ProcessCommand(request);
                await responseStream.WriteAsync(retVal);
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
            // Handle cancellation exception
            _logger.LogInformation("Client canceled the bidirectional stream.");
        }
        catch (Exception ex)
        {
            // Handle exceptions related to gRPC communication
            _logger.LogError(ex, "An error occurred while processing the bidirectional stream.");
        }
        finally
        {
            _activeClients.TryRemove(clientId, out var _);

        }
    }

    private async Task<CommandResponseStatus> ProcessCommand(CommandResponse request)
    {
        var retVal = new CommandResponseStatus
        {
            RequestId = request.RequestId,
            Status = "Processed"
        };

        try
        {
            // check if command succeded or failed and return the response, for now just log the response, might need to add it again to the Queue
            switch (request.ResponseDataCase)
            {
                case CommandResponse.ResponseDataOneofCase.ExecuteProcessResponse:
                    Console.WriteLine($"Received command from client: {request.RequestId}, ExecuteProcessResponse: {request.ExecuteProcessResponse}");
                    break;
                case CommandResponse.ResponseDataOneofCase.ReadFileResponse:
                    await _fileManager.ReadFileExecuter(request.ReadFileResponse.FileContents.ToByteArray(), request.ReadFileResponse.FileName);
                    Console.WriteLine($"Received command from client: {request.RequestId}, ReadFileResponse: {request.ReadFileResponse}");
                    break;
                case CommandResponse.ResponseDataOneofCase.WriteFileResponse:
                    Console.WriteLine($"Received command from client: {request.RequestId}, WriteFileResponse: {request.WriteFileResponse}");
                    break;
                default:
                    Console.WriteLine($"Received command from client: {request.RequestId}, Unknown response data");
                    break;
            }
            await Task.Delay(2000);


            return retVal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            retVal.Status = "Failed";
        }

        return retVal;
    }



    public static void AddCommand(string clientId, CommandRequest command)
    {
        lock (lockObject)
        {
            if (!_pendingCommands.ContainsKey(clientId))
            {
                _pendingCommands.Add(clientId, new List<CommandRequest>() { command });
            }
            _pendingCommands[clientId].Add(command);
        }
    }
    public static void AddCommands(string clientId, List<CommandRequest> commands)
    {
        lock (lockObject)
        {
            if (!_pendingCommands.ContainsKey(clientId))
            {
                _pendingCommands.Add(clientId, commands);
            }
            _pendingCommands[clientId].AddRange(commands);
        }
    }
}