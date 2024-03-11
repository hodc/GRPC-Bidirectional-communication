using Core.ProtosLibrary;

public interface ICommanderManager
{
    // public void AddCommand(string clientId, CommandRequest command);
    public Task ProcessPendingCommands();
}