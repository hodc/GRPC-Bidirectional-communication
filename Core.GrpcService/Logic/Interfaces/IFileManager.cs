public interface IFileManager
{
    public Task ReadFileExecuter(byte[] content, string fileName);
}