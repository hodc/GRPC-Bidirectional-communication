using System.Collections.Concurrent;
using Core.ProtosLibrary;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

public class ManagerActions : IManagerActions
{
    private readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ManagerActions>();
    ConcurrentQueue<CommandRequest> _commandQueueRequest = new ConcurrentQueue<CommandRequest>();
    ConcurrentQueue<CommandResponse> _commandQueueResponse = new ConcurrentQueue<CommandResponse>();
    private readonly IClientFilesActions _clientFilesActions = new ClientFilesActions();
    public ManagerActions()
    {
    }


    public async Task Run(string ServerAddress, string clientId)
    {
        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress(ServerAddress, new GrpcChannelOptions { HttpHandler = httpHandler });


        var clientCommands = new CommandService.CommandServiceClient(channel);
        try
        {
            Console.WriteLine("Starting gRPC communication with client ID: " + clientId);
            var clientInfo = new Metadata
            {
                { "client-id", clientId },
                { "os-version", GetOSVersion() }
            };
            var cts = new CancellationTokenSource();

            Console.WriteLine("Subscribing for commands ...");


            var task = Task.Run(async () =>
            {
                using (var callSubscribeCommands = clientCommands.SubscribeForCommands(new Empty(), clientInfo, cancellationToken: cts.Token))
                {
                    try
                    {
                        await foreach (var command in callSubscribeCommands.ResponseStream.ReadAllAsync(cancellationToken: cts.Token))
                        {
                            // var command = callSubscribeCommands.ResponseStream.Current;
                            Console.WriteLine($"Received command from server: {command.CommandCase.ToString()},  {command.RequestId}");
                            _commandQueueRequest.Enqueue(command);
                        }

                    }
                    catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
                    {
                        Console.WriteLine("Stream cancelled.");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error reading response: " + ex);
                        throw;
                    }
                }
            });

            Console.WriteLine("Sending command responses ...");
            var call = clientCommands.StreamCommands(clientInfo, cancellationToken: cts.Token);


            var commandResponseTask = Task.Run(async () =>
            {
                try
                {
                    await foreach (var command in call.ResponseStream.ReadAllAsync())
                    {
                        if (command.RequestId == null)
                        {
                            Console.WriteLine("Command request ID is null");
                            return;
                        }
                        Console.WriteLine("Received command from server: " + command.RequestId + command.Status);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });


            var commandRequestTask = Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        if (_commandQueueRequest.TryDequeue(out var command))
                        {
                            Console.WriteLine("Processing command: " + command.CommandCase.ToString());
                            switch (command.CommandCase)
                            {
                                case CommandRequest.CommandOneofCase.ExecuteProcessCommand:
                                    await _clientFilesActions.ExecuteProcessCommand(_commandQueueResponse, command);
                                    break;
                                case CommandRequest.CommandOneofCase.ReadFileCommand:
                                    await _clientFilesActions.ReadFileCommand(_commandQueueResponse, command);
                                    break;
                                case CommandRequest.CommandOneofCase.WriteFileCommand:
                                    await _clientFilesActions.WriteFileCommand(_commandQueueResponse, command);
                                    break;
                                default:
                                    Console.WriteLine("Command not recognized");
                                    break;
                            }

                        }

                        if (_commandQueueResponse.Count > 0)
                        {
                            if (_commandQueueResponse.TryDequeue(out var response))
                            {
                                Console.WriteLine("Processing command: " + response.ResponseDataCase.ToString());
                                // send commandResponse
                                await call.RequestStream.WriteAsync(response);
                            }
                        }
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });
            Task.WaitAll(task);
            Task.WaitAll(commandResponseTask);
            Task.WaitAll(commandRequestTask);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
            Console.WriteLine("Stream cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading response: " + ex);
            throw;
        }
        finally
        {
            // Shutdown the gRPC channel
            await channel.ShutdownAsync();

        }
    }

    private string GetOSVersion()
    {
        return Environment.OSVersion.VersionString;
    }
}