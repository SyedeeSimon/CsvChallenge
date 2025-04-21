using FloodDetection.Models;

namespace FloodDetection.Models
{
    public class RainfallAnalysisResult
    {
        public required Device Device { get; set; }
        public required double AverageRainfall { get; set; }
        public required DeviceStatus Status { get; set; }
        public required RainfallTrend Trend { get; set; }

        public override string ToString()
        {
            return $"Device: {Device.DeviceName} ({Device.Location}), Average Rainfall: {AverageRainfall}, Status: {Status}, Trend: {Trend}";
        }
    }
}