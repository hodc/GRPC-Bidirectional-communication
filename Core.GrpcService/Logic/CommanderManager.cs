using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Tasks;
using Core.ProtosLibrary;
using Google.Protobuf;

public class CommanderManager : ICommanderManager
{
    private readonly ConcurrentDictionary<string, BlockingCollection<CommandRequest>> _pendingCommands;
    private readonly IConfiguration _configurationManager;

    public CommanderManager(IConfiguration configurationManager)
    {
        _pendingCommands = new ConcurrentDictionary<string, BlockingCollection<CommandRequest>>();
        _configurationManager = configurationManager;
        // Start a task to process pending commands
        Task.Run(ProcessPendingCommands);
        Console.WriteLine("CommanderManager initialized");
    }

    public async Task ProcessPendingCommands()
    {
        try
        {
            var fileLocation = _configurationManager.GetValue<string>("commandsFile") ?? "";
            if (string.IsNullOrEmpty(fileLocation))
            {
                Console.WriteLine("No commands file location found in configuration");
                return;
            }
            var commands1 = new CommandRequest
            {
                RequestId = Guid.NewGuid().ToString(),
                WriteFileCommand = new WriteFileCommand
                {
                    FilePath = "test.txt",
                    Content = ByteString.CopyFromUtf8("test")
                }
            };
            CommandManagerItem commandManagerItem = new CommandManagerItem()
            {
                ClientId = "aaaa-aaaa-aaaa-aaaa",
                Commands = commands1
            };

            var toJson = JsonSerializer.Serialize(commandManagerItem);

            var data = await System.IO.File.ReadAllTextAsync(fileLocation);
            var commands = JsonSerializer.Deserialize<List<CommandManagerItem>>(data);
            if (commands == null)
            {
                Console.WriteLine("No commands found in file");
                return;
            }
            foreach (var command in commands)
            {
                CommandsService.AddCommand(command.ClientId, command.Commands);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in command processing loop: {ex.Message}");
        }
    }

}
