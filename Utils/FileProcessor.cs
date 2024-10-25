using System.Text.Json;
using OfficeOpenXml;
using Handshakes_Carpark.Models;
using Handshakes_Carpark.Utils;
using Handshakes_Carpark.Data;

public class FileProcessor : IFileProcessor
{
    private ApplicationDbContext _dbContext;
    public FileProcessor(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<CarPark>> FileProcessorAsync(string filePath)
    {
        // get extension of file (currently applicable to .json and .xlsx)
        var fileExtension = Path.GetExtension(filePath).ToLower();

        return fileExtension switch
        {
            // if file is json, call this method
            ".json" => await ProcessJsonFileAsync(filePath),
            // if file is excel, call this method
            ".xlsx" => await ProcessExcelFileAsync(filePath),
            _ => throw new NotSupportedException($"File type '{fileExtension}' is not supported.")
        };
    }

    private async Task<List<CarPark>> ProcessJsonFileAsync(string filePath)
    {
        // Read all text in json
        var jsonData = await File.ReadAllTextAsync(filePath);
        // Converts jsonData into a list of CarPark objects. 
        var carParks = JsonSerializer.Deserialize<List<CarPark>>(jsonData);

        if (carParks == null)
        {
            throw new Exception("No data found in the JSON file.");
        }

        return carParks;
    }

    private async Task<List<CarPark>> ProcessExcelFileAsync(string filePath)
    {
        var carParks = new List<CarPark>();

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            if (package.Workbook.Worksheets.Count == 0)
            {
                throw new Exception("The Excel file does not contain any data.");
            }

            var excelFile = package.Workbook.Worksheets[0];
            var rowCount = excelFile.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var carParkNo = excelFile.Cells[row, 1].Text;
                var existingCarPark = await _dbContext.CarParks.FindAsync(carParkNo);

                // Create new CarPark object
                var carPark = new CarPark
                {
                    CarParkNo = carParkNo,
                    Address = excelFile.Cells[row, 2].Text,
                    XCoord = double.TryParse(excelFile.Cells[row, 3].Text, out double x) ? x : 0,
                    YCoord = double.TryParse(excelFile.Cells[row, 4].Text, out double y) ? y : 0,
                    CarParkType = excelFile.Cells[row, 5].Text,
                    ParkingSystemType = excelFile.Cells[row, 6].Text,
                    ShortTermParking = excelFile.Cells[row, 7].Text,
                    FreeParking = excelFile.Cells[row, 8].Text,
                    NightParking = excelFile.Cells[row, 9].Text == "YES",
                    CarParkDecks = int.TryParse(excelFile.Cells[row, 10].Text, out int decks) ? decks : 0,
                    GantryHeight = double.TryParse(excelFile.Cells[row, 11].Text, out double height) ? height : 0,
                    CarParkBasement = excelFile.Cells[row, 12].Text == "Y"
                };

                if (existingCarPark != null)
                {
                    // Update existing fields if there are any changes
                    existingCarPark.Address = carPark.Address;
                    existingCarPark.XCoord = carPark.XCoord;
                    existingCarPark.YCoord = carPark.YCoord;
                    existingCarPark.CarParkType = carPark.CarParkType;
                    existingCarPark.ParkingSystemType = carPark.ParkingSystemType;
                    existingCarPark.ShortTermParking = carPark.ShortTermParking;
                    existingCarPark.FreeParking = carPark.FreeParking;
                    existingCarPark.NightParking = carPark.NightParking;
                    existingCarPark.CarParkDecks = carPark.CarParkDecks;
                    existingCarPark.GantryHeight = carPark.GantryHeight;
                    existingCarPark.CarParkBasement = carPark.CarParkBasement;

                    // Since car park exists, update it directly
                    _dbContext.CarParks.Update(existingCarPark);
                }
                else
                {
                    // If car park does not exist, add to list
                    carParks.Add(carPark);
                }
            }
        }

        return carParks;
    }

}
