using Grpc.Core;
using Core.ProtosLibrary;


public class FilesActionsService : FileActionsService.FileActionsServiceBase
{
    private readonly ILogger<FilesActionsService> _logger;
    private Dictionary<string, MemoryStream> fileStreams = new Dictionary<string, MemoryStream>();

    public FilesActionsService(ILogger<FilesActionsService> logger)
    {
        _logger = logger;
    }

    public override async Task<fileUploadResponse> GetFile(IAsyncStreamReader<fileUploadRequest> request, ServerCallContext context)
    {
        string fileName = String.Empty;

        while (await request.MoveNext())
        {
            var fileChunk = request.Current;
            fileName = fileChunk.FileName;
            if (fileChunk.FileName == null)
            {
                Console.WriteLine("File name is null");
                return new fileUploadResponse
                {
                    Status = "File name is null"
                };
            }

            else
            {
                byte[] fileData = fileChunk.Content.ToByteArray();

                Console.WriteLine($"Received file: {fileName}");
                // Process or save the file on the server
                ProcessReceivedFile(fileName, fileData);
            }
        }

        // SaveFileToDisk(fileName);

        return new fileUploadResponse
        {
            Status = "File uploaded successfully"
        };
    }

    private void ProcessReceivedFile(string fileName, byte[] fileData)
    {
        File.WriteAllBytes("./ReceivedFiles/" + fileName, fileData);
        // fileStreams[fileName].Write(fileData, 0, fileData.Length);
        Console.WriteLine($"Received file chunk for {fileName}");

    }

}