public class FileManager : IFileManager
{
    private readonly ILogger<FileManager> _logger;
    public FileManager(ILogger<FileManager> logger)
    {
        _logger = logger;
    }

    public async Task ReadFileExecuter(byte[] content, string fileName)
    {
        try
        {
            File.WriteAllBytes(fileName, content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}