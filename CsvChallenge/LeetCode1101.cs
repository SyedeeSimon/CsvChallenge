using CsvHelper;
    using System.Globalization;
    using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using CsvChallenge.Models;

namespace LeetCode
{

    public class Solution
    {
        public int EarliestAcq(int[][] logs, int n)
        {

            var testData = new TestData
            {
                N = n,
                Logs = logs.Select(log => new Log
                {
                    Timestamp = log[0],
                    Person1 = log[1],
                    Person2 = log[2]
                }).ToList()
            };

            return EarliestAcq(testData);
        }

        public int EarliestAcq(TestData testData)
        {
            // At least N-1 edges are needed to connect N nodes
            if (testData.Logs.Count < testData.N - 1)
            {
                return -1;
            }
            // Sort the logs by timestamp
            testData.Logs = testData.Logs.OrderBy(log => log.Timestamp).ToList();

            for (int i = testData.N - 1; i <= testData.Logs.Count; i++)
            {
                var subList = testData.Logs.Take(i).ToList();
                int connectedElements = CountConnectedNodes(subList);
                if (connectedElements == testData.N) return testData.Logs[i - 1].Timestamp;
             
            }
            return -1;
        }
        public void PrintLogs(List<Log> logs)
        {
            Console.WriteLine("Logs:");
            foreach (var log in logs)
            {
            Console.WriteLine($"Person1: {log.Person1}, Person2: {log.Person2}, Timestamp: {log.Timestamp}");
            }
        }
        

        public int CountConnectedNodes(List<Log> logs)
        {
            // Determine the distinct nodes in the logs
            var nodes = new HashSet<int>();
            foreach (var log in logs)
            {
            nodes.Add(log.Person1);
            nodes.Add(log.Person2);
            }

            // Create an adjacency list for the graph
            var graph = new Dictionary<int, List<int>>();
            foreach (var node in nodes)
            {
            graph[node] = new List<int>();
            }

            foreach (var log in logs)
            {
            graph[log.Person1].Add(log.Person2);
            graph[log.Person2].Add(log.Person1);
            }

            // Perform DFS from any one node
            var visited = new HashSet<int>();
            var startNode = nodes.First();
            DFS(startNode, graph, visited);

            // Return the number of visited nodes
            return visited.Count;
        }
        private void DFS(int node, Dictionary<int, List<int>> graph, HashSet<int> visited)
        {
            visited.Add(node);

            foreach (var neighbor in graph[node])
            {
            if (!visited.Contains(neighbor))
            {
                DFS(neighbor, graph, visited);
            }
            }
        }
        
        public TestData ReadTestFile(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("No file path provided in arguments.");
            }

            var fileName = args[0];

            var n = ValidateAndGetN(fileName);
            
            var logsList = new List<Log>();
            var filePath = Path.Combine("Resources", "CsvFiles", fileName);
            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                logsList = csv.GetRecords<Log>().ToList();
            }
            return new TestData
            {
                N = n,
                Logs = logsList
            };
        }

        private int ValidateAndGetN(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var pattern = @"^([a-zA-Z0-9]+)_(\d+)\.csv$";
            var match = Regex.Match(fileName, pattern);

            if (!match.Success)
            {
                throw new ArgumentException("File name is not in the expected format 'prefix_{n}.csv'.");
            }

            if (!int.TryParse(match.Groups[2].Value, out int n))
            {
                throw new ArgumentException("The value of {n} in the file name is not a valid integer.");
            }

            return n;
        }




        public static void Main(string[] args)
        {
            args = new string[] { "testCase2_6.csv" };
            var solution = new Solution();
            var testData = solution.ReadTestFile(args);
            var result = solution.EarliestAcq(testData);
            Console.WriteLine($"Earliest time when all people are connected: {result}");
        }
    }
}
