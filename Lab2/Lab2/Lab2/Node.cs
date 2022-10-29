namespace Lab2;

public class Node
{
    private Node? parent;

    public Board State { get; }
    public int Depth { get; }
    public Node[] Successors { get; set; }
    
    internal int Cost() => Depth + State.CountConfs();
    
    public Node(Board InitialState)
    {
        parent = null;
        State = InitialState;
        Depth = 0;
    }
    
    public Node(ref Node parentNode, int queenY, byte dest)
    {
        parent = parentNode;
        Depth = parentNode.Depth + 1;
        State = new Board(parentNode.State.GetSize(), parentNode.State);
        State.MoveQueen(queenY, dest);
    }

    public override string ToString()
    {
        return State.ToString();
    }

    public static void Expand(Node node)
    {
        int index = 0;
        node.Successors = new Node[node.State.GetSize() * (node.State.GetSize() - 1)];
        for (int i = 0; i < node.State.GetSize(); i++)
        {
            for (byte j = 0; j < node.State.GetSize(); j++)
            {
                if(node.State.GetRow(i) == j) continue;
                node.Successors[index++] = new Node(ref node, i, j);
            }
        }
    }

    
}