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
            Generator.Generate(Constants.initFilePath, integersToGen);
            Stopwatch sw = Stopwatch.StartNew();
            BasicMWayMerge mwm = new BasicMWayMerge(integersToGen, mOffiles);
            mwm.Sort();
            sw.Stop();
            Console.WriteLine($"Done in {sw.ElapsedMilliseconds}");
            Console.WriteLine($"Sorted data is stored in {mwm.initFilePath}");
            Console.Write("Do you want to check if file is sorted? [Y/N]? ");
            string? answer = Console.ReadLine();
            if (answer.Contains("Y"))
            {
                Console.WriteLine(mwm.CheckIfSorted());
            }
        }
    }