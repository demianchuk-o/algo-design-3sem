namespace Lab4;

class Program
{
    public static void Main(string[] args)
    {
        int iterations = 12000;
        GeneticAlgorithm ga = new GeneticAlgorithm();
        ga.Start(iterations);
    }
}