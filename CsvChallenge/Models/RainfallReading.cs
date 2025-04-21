using CsvHelper.Configuration.Attributes;

namespace FloodDetection.Models
{
    public class RainfallReading
    {
        [Name("Device ID")]
        public int DeviceId { get; set; }

        [Name("Time")]
        public DateTime Time { get; set; }

        [Name("Rainfall")]
        public double Rainfall { get; set; }
    }
}
