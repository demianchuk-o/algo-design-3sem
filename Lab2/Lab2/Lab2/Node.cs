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
    
    public Node(ref Node parentNode, int queenX, int queenY, int dest)
    {
        parent = parentNode;
        depth = parentNode.depth + 1;
        state = new Board(parentNode.state.size, parentNode.state);
        state.MoveQueen(queenX, queenY, dest);
    }

    public override string ToString()
    {
        return state.ToString();
    }

    public static void Expand(Node node)
    {
        int index = 0;
        node.successors = new Node[node.state.size * (node.state.size - 1)];
        for (int col = 0; col < node.state.size; col++)        
        {
            for (int row = 0; row < node.state.size; row++)
            {
                if (node.state.GetFieldTile(row, col))
                {
                    for (int y = 0; y < node.state.size; y++)
                    {
                        if (y == row)
                        {
                            continue;
                        }

                        node.successors[index++] = new Node(ref node, row, col, y);
                    }
                }
            }
        }
    }

}