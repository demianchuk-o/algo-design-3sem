using System.Diagnostics;

namespace Lab5;

class Program
{
    public static void Main(string[] args)
    {
        Graph gr = new Graph();
        AntColony antColony = new AntColony(g: gr, a: 1d, b: 1d, r: 0.5, lm: 500, true, elite: 0, reg: 10, wild: 0);
        Stopwatch stopwatch = Stopwatch.StartNew();
        antColony.Start(200);
        Console.WriteLine($"Done in {stopwatch.Elapsed}");
        Console.WriteLine("Best path is: ");
        antColony.WritePath();
        antColony.SaveResults();
        Console.WriteLine($"All the other info is written into {AntColony.filepath}");
    }
}