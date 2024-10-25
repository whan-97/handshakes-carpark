using Handshakes_Carpark.Data;
using Handshakes_Carpark.Utils;

public class BatchJob
{
    private readonly ApplicationDbContext _context;
    private readonly IFileProcessor _fileProcessor;
    public BatchJob(ApplicationDbContext context, IFileProcessor fileProcessor)
    {
        _context = context;
        _fileProcessor = fileProcessor;
    }

    public async Task ProcessBatchFileAsync(string filePath)
    {
        // Call the custom function to process the file
        // More reusable and flexible
        var carParks = await _fileProcessor.FileProcessorAsync(filePath);

        await _context.AddRangeAsync(carParks);
        await _context.SaveChangesAsync();
    }
}
      
