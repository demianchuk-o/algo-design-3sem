namespace Lab2;

public class Node
{
    private Node parent;

    public Board state { get; }
    public int depth { get; init; }
    public Node[] successors { get; set; }
    
    public Node(Board InitialState)
    {
        parent = null;
        state = InitialState;
        depth = 0;
    }
    
    public Node(ref Node parentNode, int queenY, byte dest)
    {
        parent = parentNode;
        depth = parentNode.depth + 1;
        state = new Board(parentNode.state.GetSize(), parentNode.state);
        state.MoveQueen(queenY, dest);
    }

    public override string ToString()
    {
        return state.ToString();
    }

    public static void Expand(Node node)
    {
        int index = 0;
        node.successors = new Node[node.state.GetSize() * (node.state.GetSize() - 1)];
        for (int i = 0; i < node.state.GetSize(); i++)
        {
            for (byte j = 0; j < node.state.GetSize(); j++)
            {
                if(node.state.GetRow(i) == j) continue;
                node.successors[index++] = new Node(ref node, i, j);
            }
        }
    }

}