namespace FloodDetection.Models
{
    public class DeviceRainfallData
    {
        public required Device Device { get; set; }
        public List<RainfallReading> RainfallReadings { get; set; } = new List<RainfallReading>();
    }
}