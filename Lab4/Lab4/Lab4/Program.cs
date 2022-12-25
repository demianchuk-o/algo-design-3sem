namespace Lab4;

class Program
{
    public static void Main(string[] args)
    {
        int iterations = Int32.Parse(Console.ReadLine());
        GeneticAlgorithm ga = new GeneticAlgorithm();
        ga.Start(iterations);
    }
}