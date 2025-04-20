using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace ReadWriteCsv
{

    class Person
    {
        [Name("name")]
        public string? Name { get; set; }

        [Name("age")]
        public int Age { get; set; }

        [Name("sex")]
        public string? Sex { get; set; }

        [Name("country")]
        public string? Country { get; set; }
    }

    class ReaderWriter
    {


        static void main(string[] args)
        {

            var path = Path.Combine("Resources", "CsvFiles", "persons.csv");

            using (var reader = new StreamReader(path))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var persons = csvReader.GetRecords<Person>().ToList();

                foreach (string header in csvReader.HeaderRecord)
                {
                    Console.Write($"{header}, ");
                }
                Console.WriteLine();

                foreach (Person person in persons)
                {
                    Console.WriteLine($"{person.Name}, {person.Age}, {person.Sex}, {person.Country}");
                }

                // Write to a new CSV file 
                var newPath = Path.Combine("Resources", "CsvFiles", "new_persons.csv");
                using (var writer = new StreamWriter(newPath))
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteRecords(persons);
                }

            }


        }

    }

}