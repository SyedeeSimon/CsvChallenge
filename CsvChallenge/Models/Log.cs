using CsvHelper.Configuration.Attributes;


namespace CsvChallenge.Models
{

    public class Log
    {
        [Index(1)]
        public int Person1 { get; set; }

        [Index(2)]
        public int Person2 { get; set; }

        [Index(0)]
        public int Timestamp { get; set; }
    }
}