using FloodDetection.Models;

namespace FloodDetection.Services
{
    public class RainfallAnalysisService
    {
        private static readonly int AnalyticsWindow = 4;

        private const double GreenRainfallThreshold = 10.0;
        private const double AmberRainfallThreshold = 15.0;

        public static DateTime GetLatestTime(List<RainfallReading> rainfallReadings)
        {
            if (!rainfallReadings.Any())
            {
                throw new InvalidOperationException("The list of rainfall readings is empty.");
            }

            // Return the largest (most recent) time
            return rainfallReadings.Max(r => r.Time);
        }

        public double GetAverageRainfall(List<RainfallReading> rainfallReadings, DateTime latestTime)
        {
            // Filter readings that fall within the range [latestTime - 4 hours, latestTime]
            var recentReadings = rainfallReadings
                .Where(r => r.Time >= latestTime.AddHours(-AnalyticsWindow) && r.Time <= latestTime)
                .ToList();

            return recentReadings.Any() ? recentReadings.Average(r => r.Rainfall) : 0.0;
        }

        public DeviceStatus GetDeviceStatus(List<RainfallReading> rainfallReadings, DateTime latestTime, double averageRainfall)
        {
            // Filter readings from the last 4 hours
            var recentReadings = rainfallReadings
                .Where(r => r.Time >= latestTime.AddHours(-AnalyticsWindow) && r.Time <= latestTime)
                .ToList();

            // If there is at least one reading greater than 30, return Red
            if (recentReadings.Any(r => r.Rainfall > 30))
            {
                return DeviceStatus.Red;
            }

            // Determine the device status based on the average rainfall
            if (averageRainfall < GreenRainfallThreshold)
            {
                return DeviceStatus.Green;
            }
            else if (averageRainfall < AmberRainfallThreshold)
            {
                return DeviceStatus.Amber;
            }
            else
            {
                return DeviceStatus.Red;
            }
        }

        public RainfallTrend GetRainfallTrend(double currentWindowAverage, double previousWindowAverage)
        {
            if (currentWindowAverage > previousWindowAverage)
            {
                return RainfallTrend.Increasing;
            }
            else if (currentWindowAverage < previousWindowAverage)
            {
                return RainfallTrend.Decreasing;
            }

            return RainfallTrend.Stable;
        }

        public RainfallAnalysisResult GetRainfallAnalysisResult(DeviceRainfallData deviceRainfallData, DateTime currentTime)
        {
            // Calculate the current window average
            var currentWindowAverage = GetAverageRainfall(deviceRainfallData.RainfallReadings, currentTime);

            // Calculate the previous window average (currentTime - 4 hours)
            var previousWindowAverage = GetAverageRainfall(deviceRainfallData.RainfallReadings, currentTime.AddHours(-AnalyticsWindow));

            // Determine the status based on the current window average
            var status = GetDeviceStatus(deviceRainfallData.RainfallReadings, currentTime, currentWindowAverage);

            // Determine the trend based on the two averages
            var trend = GetRainfallTrend(currentWindowAverage, previousWindowAverage);

            // Create and return the RainfallAnalysisResult
            return new RainfallAnalysisResult
            {
                Device = deviceRainfallData.Device,
                AverageRainfall = currentWindowAverage,
                Status = status,
                Trend = trend
            };
        }
    }
}