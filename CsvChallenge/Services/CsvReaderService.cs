using CsvHelper;
using FloodDetection.Models;
using System.Globalization;

namespace FloodDetection.Services
{
    public class RainfallDataService
    {
        private const string DeviceDataFilePath = "Resources/Data/DeviceData/Devices.csv";
        private const string RainfallReadingDataDirectory = "Resources/Data/RainfallReadingData";

        public List<Device> LoadDevices()
        {
            using (var reader = new StreamReader(DeviceDataFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Device>().ToList();
            }
        }

        public List<RainfallReading> LoadRainfallData()
        {
            var rainfallReadings = new List<RainfallReading>();

            // Get all CSV files in the RainfallReadingData directory
            var files = Directory.GetFiles(RainfallReadingDataDirectory, "*.csv");

            foreach (var file in files)
            {
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    rainfallReadings.AddRange(csv.GetRecords<RainfallReading>());
                }
            }

            return rainfallReadings;
        }

        public List<DeviceRainfallData> GroupRainfallDataByDevice(List<Device> devices, List<RainfallReading> rainfallReadings)
        {
            var groupedData = devices.Select(device => new DeviceRainfallData
            {
                Device = device,
                RainfallReadings = rainfallReadings
                    .Where(reading => reading.DeviceId == device.DeviceId)
                    .ToList()
            }).ToList();

            return groupedData;
        }
    }
}