using Handshakes_Carpark.Models;

namespace Handshakes_Carpark.Utils
{
    public interface IFileProcessor
    {
        Task<List<CarPark>> FileProcessorAsync(string filePath);
    }
}
