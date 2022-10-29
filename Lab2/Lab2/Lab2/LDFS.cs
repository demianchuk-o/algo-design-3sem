namespace Lab2;

public class LDFS
{
    private StatsHandler _statsHandler;
    private HashSet<Board> TotalStates;
    private HashSet<Board> StatesInMemory;
    public LDFS(Node node, int lim, bool choice)
    {
        TotalStates = new HashSet<Board>();
        StatesInMemory = new HashSet<Board>();
        _statsHandler = new StatsHandler(choice);
        _statsHandler.Stopwatch.Start();
        _statsHandler.Solution = DepthLimitedSearch(node, Status.SOLVING, lim);
        _statsHandler.Stopwatch.Stop();
        _statsHandler.StatesInMemory = StatesInMemory.Count;
        _statsHandler.TotalStates = TotalStates.Count;
    }

    

    private Tuple<Node?, Status> DepthLimitedSearch(Node node, Status status, int lim)
    {
        return RecursiveDLS(node, Status.SOLVING, lim);
    }
    
    private Tuple<Node?, Status> RecursiveDLS(Node node, Status status, int lim)
    {
        ++_statsHandler.Iterations;
        if (_statsHandler.CheckLimits)
        {
            if (_statsHandler.Stopwatch.ElapsedMilliseconds >= 1800000)
            {
                ++_statsHandler.DeadEnds;
                return new Tuple<Node?, Status>(null, Status.TIME_EXCEEDED);
            }
        }
        bool cutoffOccured = false;
        if (node.State.CountConfs() == 0)
            return new Tuple<Node?, Status>(node, Status.SUCCESS);
        if (node.Depth == lim)
        {
            ++_statsHandler.DeadEnds;
            return new Tuple<Node?, Status>(null, Status.CUTOFF);
        }
        TotalStates.Add(node.State);
        Node.Expand(node);
        foreach (var successor in node.Successors)
        {
            TotalStates.Add(successor.State);
            Tuple<Node?, Status> result = RecursiveDLS(successor, Status.SOLVING, lim);
            if (result.Item2 == Status.TIME_EXCEEDED)
            {
                return result;
            }
            if (result.Item2 == Status.CUTOFF)
            {
                cutoffOccured = true;
                
            }

            else if (result.Item2 != Status.FAILURE)
            {
                foreach (var _successor in node.Successors)
                {
                    StatesInMemory.Add(_successor.State);
                }
                return result;
            }
        }

        if (cutoffOccured)
        {
            return new Tuple<Node?, Status>(null, Status.CUTOFF);
        }
        
        return new Tuple<Node?, Status>(null, Status.FAILURE);
    }

    public string DisplayResults()
    {
        return _statsHandler.Results() + _statsHandler.Stats();
    }
}