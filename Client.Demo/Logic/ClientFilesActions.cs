using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Core.ProtosLibrary;
using Google.Protobuf;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

public class ClientFilesActions : IClientFilesActions
{
    private ILogger<ClientFilesActions> _logger = new LoggerFactory().CreateLogger<ClientFilesActions>();
    public ClientFilesActions()
    {
        
    }
    public async Task ReadFileCommand(ConcurrentQueue<CommandResponse> _commandQueueResponse, CommandRequest command)
    {
        try
        {

            var fileContent = File.ReadAllBytes(command.ReadFileCommand.FilePath);
            _commandQueueResponse.Enqueue(new CommandResponse()
            {
                RequestId = command.RequestId,
                ReadFileResponse = new ReadFileResponse()
                {
                    FileContents = ByteString.CopyFrom(fileContent)
                }
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
    public async Task ExecuteProcessCommand(ConcurrentQueue<CommandResponse> _commandQueueResponse, CommandRequest command)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command.ExecuteProcessCommand.ProcessName,
                    Arguments = string.Join(" ", command.ExecuteProcessCommand.ProcessArguments),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            process.Start();
            process.WaitForExit();
            var output = process.StandardOutput.ReadToEnd();
            Console.WriteLine("Process output: " + output);
            _commandQueueResponse.Enqueue(new CommandResponse()
            {
                RequestId = command.RequestId,
                ExecuteProcessResponse = new ExecuteProcessResponse()
                {
                    Stderr = "",
                    Stdout = output,
                    ExitCode = process.ExitCode
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public Task WriteFileCommand(ConcurrentQueue<CommandResponse> commandQueueResponse, CommandRequest command)
    {
        try
        {
            File.WriteAllBytes(command.WriteFileCommand.FilePath, command.WriteFileCommand.Content.ToByteArray());
            commandQueueResponse.Enqueue(new CommandResponse()
            {
                RequestId = command.RequestId,
                WriteFileResponse = new WriteFileResponse()
                {
                    Success = true
                }
            });
            return Task.CompletedTask;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }

    }
}
