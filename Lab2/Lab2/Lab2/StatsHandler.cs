using System.Diagnostics;

namespace Lab2;

public class StatsHandler
{
    public Tuple<Node?, Status> Solution { get; set; }
    public long Iterations { get; set; }
    
    public int DeadEnds { get; set; }
    public bool CheckLimits { get; }
    public Stopwatch Stopwatch { get; }
   
    
    public long TotalStates { get; set; }
    
    public long StatesInMemory { get; set; }

    public StatsHandler(bool checkLimits)
    {
        Iterations = 0;
        DeadEnds = 0;
        TotalStates = 0;
        StatesInMemory = 0;
        CheckLimits = checkLimits;
        Stopwatch = new Stopwatch();
    }
    public string Results()
    {
        return Solution.Item2 switch
        {
            Status.SUCCESS => "Solution found successfully!\n" + Solution.Item1,
            Status.CUTOFF => "The search was cut off.\n",
            Status.FAILURE => "The search has failed.\n",
            Status.TIME_EXCEEDED => "The search has exceeded the time limit.",
        };
    }

    public string Stats() => $"Iterations: {Iterations}\nTime elapsed: {Stopwatch.ElapsedMilliseconds} ms\nDead ends: {DeadEnds}\nTotal states: {TotalStates}\nStates in memory: {StatesInMemory}" ;
    
}