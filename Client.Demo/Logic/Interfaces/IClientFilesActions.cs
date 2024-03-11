using System.Collections.Concurrent;
using Core.ProtosLibrary;

public interface IClientFilesActions
{
    public Task ExecuteProcessCommand(ConcurrentQueue<CommandResponse> _commandQueueResponse, CommandRequest command);

    public Task ReadFileCommand(ConcurrentQueue<CommandResponse> _commandQueueResponse, CommandRequest command);
    Task WriteFileCommand(ConcurrentQueue<CommandResponse> commandQueueResponse, CommandRequest command);
}