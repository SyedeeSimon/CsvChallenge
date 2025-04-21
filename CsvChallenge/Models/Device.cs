using CsvHelper.Configuration.Attributes;

namespace FloodDetection.Models
{
    public class Device
    {
        [Name("Device ID")]
        public int DeviceId { get; set; }

        [Name("Device Name")]
        public required string DeviceName { get; set; }

        [Name("Location")]
        public required string Location { get; set; }
    }
}
