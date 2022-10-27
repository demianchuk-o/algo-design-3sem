namespace Lab2;

public class LDFS
{
    private Tuple<Node?, Status> solution { get; set; }

    public LDFS(Node node, int lim)
    {
        solution = DepthLimitedSearch(node, Status.SOLVING, lim);
    }

    public override string ToString()
    {
        return solution.Item2 switch
        {
            Status.SUCCESS => "Solution found successfully!\n" + solution.Item1,
            Status.CUTOFF => "The search was cut off.",
            Status.FAILURE => "The search has failed :(",
        };
    }

    private static Tuple<Node?, Status> DepthLimitedSearch(Node node, Status status, int lim)
    {
        return RecursiveDLS(node, Status.SOLVING ,lim);
    }
    
    private static Tuple<Node?, Status> RecursiveDLS(Node node, Status status, int lim)
    {
        bool cutoffOccured = false;
        if (node.state.CountConfs() == 0)
            return new Tuple<Node?, Status>(node, Status.SUCCESS);
        if (node.depth == lim)
            return new Tuple<Node?, Status>(null, Status.CUTOFF);
        Node.Expand(node);
        foreach (var successor in node.successors)
        {
            Tuple<Node?, Status> result = RecursiveDLS(successor, Status.SOLVING, lim);
            if (result.Item2 == Status.CUTOFF)
                cutoffOccured = true;
            else if (result.Item2 != Status.FAILURE)
                return result;
        }

        if (cutoffOccured)
            return new Tuple<Node?, Status>(null, Status.CUTOFF);
        else
        {
            return new Tuple<Node?, Status>(null, Status.FAILURE);
        }
    }
}