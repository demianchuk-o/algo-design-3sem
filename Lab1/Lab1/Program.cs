using System.Diagnostics;

namespace Lab1;
internal class Program
    {
        public static void Main(string[] args)
        {
            string startFile = "A.dat";
            long integersToGen = Int64.Parse(Console.ReadLine());
            Stopwatch sw = Stopwatch.StartNew();
            Generator.Generate(startFile, integersToGen);
            sw.Stop();
            Console.WriteLine($"File generated in {sw.ElapsedMilliseconds}");
        }
        
       
    }