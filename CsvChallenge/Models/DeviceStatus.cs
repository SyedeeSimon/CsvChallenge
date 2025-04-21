namespace FloodDetection.Models
{
    public enum DeviceStatus
    {
        Green,  // Average rainfall < 10mm
        Amber,  // Average rainfall >= 10mm and < 15mm
        Red     // Average rainfall >= 15mm
    }

    public static class DeviceStatusHelper
    {
        private const double GreenThreshold = 10.0;
        private const double AmberThreshold = 15.0;

        public static DeviceStatus GetStatus(double averageRainfall)
        {
            if (averageRainfall < GreenThreshold)
            {
                return DeviceStatus.Green;
            }
            else if (averageRainfall < AmberThreshold)
            {
                return DeviceStatus.Amber;
            }
            else
            {
                return DeviceStatus.Red;
            }
        }
    }
}