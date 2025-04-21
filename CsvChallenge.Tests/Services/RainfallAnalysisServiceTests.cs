using FloodDetection.Models;
using FloodDetection.Services;
using Xunit;

namespace CsvChallenge.Tests.Services
{
    public class RainfallAnalysisServiceTests
    {
        private readonly RainfallAnalysisService _rainfallAnalysisService;

        public RainfallAnalysisServiceTests()
        {
            _rainfallAnalysisService = new RainfallAnalysisService();
        }

        [Fact]
        public void GetLatestTime_ShouldReturnMostRecentTime()
        {
            // Arrange
            var rainfallReadings = new List<RainfallReading>
            {
                new RainfallReading { Time = DateTime.Parse("2023-04-20T10:00:00") },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T12:00:00") },
                new RainfallReading { Time = DateTime.Parse("2025-04-20T11:00:00") }
            };

            // Act
            var latestTime = RainfallAnalysisService.GetLatestTime(rainfallReadings);

            // Assert
            Assert.Equal(DateTime.Parse("2025-04-20T11:00:00"), latestTime);
        }

        [Fact]
        public void GetAverageRainfall_ShouldReturnCorrectAverage()
        {
            // Arrange
            var rainfallReadings = new List<RainfallReading>
            {
                new RainfallReading { Time = DateTime.Parse("2023-04-20T07:00:00"), Rainfall = 10 },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T08:00:00"), Rainfall = 10 },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T09:00:00"), Rainfall = 30 },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T10:00:00"), Rainfall = 30 },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T11:00:00"), Rainfall = 30 },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T12:00:00"), Rainfall = 30 },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T13:00:00"), Rainfall = 30 },
            };
            var latestTime = DateTime.Parse("2023-04-20T13:00:00");

            // Act
            var averageRainfall = _rainfallAnalysisService.GetAverageRainfall(rainfallReadings, latestTime);

            // Assert
            Assert.Equal(30, averageRainfall);
        }

        [Fact]
        public void GetDeviceStatus_ShouldReturnRed_WhenRainfallExceeds30InLast4Hours()
        {
            // Arrange
            var rainfallReadings = new List<RainfallReading>
            {
                new RainfallReading { Time = DateTime.Parse("2023-04-20T01:00:00"), Rainfall = 1 },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T02:00:00"), Rainfall = 1 },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T03:00:00"), Rainfall = 1 },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T04:00:00"), Rainfall = 31 },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T05:00:00"), Rainfall = 1 },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T06:00:00"), Rainfall = 1 },
            };
            var latestTime = DateTime.Parse("2023-04-20T06:00:00");
            var averageRainfall = _rainfallAnalysisService.GetAverageRainfall(rainfallReadings, latestTime);

            // Act
            var status = _rainfallAnalysisService.GetDeviceStatus(rainfallReadings, latestTime, averageRainfall);

            // Assert
            Assert.Equal(DeviceStatus.Red, status);
        }

        [Theory]
        [InlineData(5, DeviceStatus.Green)]
        [InlineData(12, DeviceStatus.Amber)]
        [InlineData(20, DeviceStatus.Red)]
        public void GetDeviceStatus_ShouldReturnCorrectStatus_BasedOnAverageRainfall(double averageRainfall, DeviceStatus expectedStatus)
        {
            // Arrange
            var rainfallReadings = new List<RainfallReading>
            {
                new RainfallReading { Time = DateTime.Parse("2023-04-20T08:00:00"), Rainfall = 10 },
                new RainfallReading { Time = DateTime.Parse("2023-04-20T09:00:00"), Rainfall = 20 }
            };
            var latestTime = DateTime.Parse("2023-04-20T10:00:00");

            // Act
            var status = _rainfallAnalysisService.GetDeviceStatus(rainfallReadings, latestTime, averageRainfall);

            // Assert
            Assert.Equal(expectedStatus, status);
        }

        [Theory]
        [InlineData(15, 10, RainfallTrend.Increasing)]
        [InlineData(10, 15, RainfallTrend.Decreasing)]
        [InlineData(10, 10, RainfallTrend.Stable)]
        public void GetRainfallTrend_ShouldReturnCorrectTrend(double currentWindowAverage, double previousWindowAverage, RainfallTrend expectedTrend)
        {
            // Act
            var trend = _rainfallAnalysisService.GetRainfallTrend(currentWindowAverage, previousWindowAverage);

            // Assert
            Assert.Equal(expectedTrend, trend);
        }

        [Fact]
        public void GetRainfallAnalysisResult_ShouldReturnCorrectResult()
        {
            // Arrange
            var device = new Device { DeviceId = 1, DeviceName = "Gauge 1", Location = "Test Location" };
            var rainfallReadings = new List<RainfallReading>
            {
                new RainfallReading { DeviceId = 1, Time = DateTime.Parse("2023-04-20T08:00:00"), Rainfall = 10 },
                new RainfallReading { DeviceId = 1, Time = DateTime.Parse("2023-04-20T09:00:00"), Rainfall = 20 },
                new RainfallReading { DeviceId = 1, Time = DateTime.Parse("2023-04-20T10:00:00"), Rainfall = 30 }
            };
            var deviceRainfallData = new DeviceRainfallData
            {
                Device = device,
                RainfallReadings = rainfallReadings
            };
            var currentTime = DateTime.Parse("2023-04-20T10:00:00");

            // Act
            var analysisResult = _rainfallAnalysisService.GetRainfallAnalysisResult(deviceRainfallData, currentTime);

            // Assert
            Assert.NotNull(analysisResult);
            Assert.Equal(device, analysisResult.Device);
            Assert.Equal(20, analysisResult.AverageRainfall);
            Assert.Equal(DeviceStatus.Red, analysisResult.Status);
            Assert.Equal(RainfallTrend.Increasing, analysisResult.Trend);
        }
    }
}