using FloodDetection.Models;
using FloodDetection.Services;

namespace FloodDetection
{
    public class Program
    {
        private static readonly RainfallDataService _rainfallDataService = new RainfallDataService();
        private static readonly RainfallAnalysisService _rainfallAnalysisService = new RainfallAnalysisService();

        public static void Main(string[] args)
        {
            // Step 1: Load devices
            var devices = _rainfallDataService.LoadDevices();
            Console.WriteLine("Devices loaded successfully.");

            // Step 2: Load rainfall data
            var rainfallData = _rainfallDataService.LoadRainfallData();
            Console.WriteLine("Rainfall data loaded successfully.");

            // Step 3: Get the latest time
            var currentTime = RainfallAnalysisService.GetLatestTime(rainfallData);
            Console.WriteLine($"Current time determined: {currentTime}");

            // Step 4: Group rainfall data by device
            var groupedData = _rainfallDataService.GroupRainfallDataByDevice(devices, rainfallData);
            Console.WriteLine("Rainfall data grouped by device.");

            // Step 5: Analyze rainfall data for each device
            Console.WriteLine("\nRainfall Analysis Results:");
            foreach (var deviceRainfallData in groupedData)
            {
                var analysisResult = _rainfallAnalysisService.GetRainfallAnalysisResult(deviceRainfallData, currentTime);
                Console.WriteLine(analysisResult);
            }
        }
    }
}