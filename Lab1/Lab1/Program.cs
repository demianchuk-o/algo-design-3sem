using System.Diagnostics;
namespace Lab1;
internal class Program
    {
        public static void Main(string[] args)
        {
            
            Console.Write("Enter the amount of Integers: ");
            long integersToGen = Int64.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.Write("Enter the number of files (m): ");
            int mOffiles = Int32.Parse(Console.ReadLine());
            Stopwatch sw = Stopwatch.StartNew();
            Generator.Generate(Constants.initFilePath, integersToGen);
            BasicMultiWayMerge mwm = new BasicMultiWayMerge(integersToGen, mOffiles);
            mwm.Sort();
            sw.Stop();
            Console.WriteLine($"Done in {sw.ElapsedMilliseconds}");
            
        }
        
       
    }