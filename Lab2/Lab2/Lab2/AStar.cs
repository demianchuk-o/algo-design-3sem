namespace Lab2;

public class AStar
{
    private StatsHandler _statsHandler;

    public AStar(Node problem, bool choice)
    {
        _statsHandler = new StatsHandler(choice);
        _statsHandler.Stopwatch.Start();

        _statsHandler.Solution = AStarSearch(problem);
        _statsHandler.Stopwatch.Stop();

    }

    private Tuple<Node?, Status> AStarSearch(Node problem)
    {
        if (_statsHandler.CheckLimits)
        {
            if (_statsHandler.Stopwatch.ElapsedMilliseconds >= 1800000)
            {
                ++_statsHandler.DeadEnds;
                return new Tuple<Node?, Status>(null, Status.TIME_EXCEEDED);
            }
        }
        
        PriorityQueue<Node, int> open = new PriorityQueue<Node, int>();
        HashSet<Board> closed = new HashSet<Board>();
        open.Enqueue(problem, problem.Cost());
        while (open.Count != 0)
        {
            Node current = open.Dequeue();
            if (current.State.CountConfs() == 0)
            {
                _statsHandler.TotalStates = _statsHandler.StatesInMemory = open.Count + closed.Count;
                return new Tuple<Node?, Status>(current, Status.SUCCESS);
            }
            closed.Add(current.State);
            ++_statsHandler.Iterations;
            Node.Expand(current);
            foreach (var successor in current.Successors)
            {
                if(!closed.Contains(successor.State))
                    open.Enqueue(successor, successor.Cost());
            }
        }

        return new Tuple<Node?, Status>(null, Status.FAILURE);
    }

    public string DisplayResults()
    {
        return _statsHandler.Results() + _statsHandler.Stats();
    }
    
}