namespace Lab2;

internal class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Console.WriteLine();
            Console.Write("Enter the size of board: ");
            int size = Int32.Parse(Console.ReadLine());
            Console.WriteLine();
            Board brd = new Board(size);
            int x, y;
            int queens = 0;
            while(queens < brd.size)
            {
                Console.Write($"Place the {queens+1} Queen: ");
                Console.Write("\tX: ");
                x = Int32.Parse(Console.ReadLine());
                Console.Write("\t\t\tY: ");
                y = Int32.Parse(Console.ReadLine());
                if (brd.AddQueen(x, y))
                {
                    Console.WriteLine($"New Queen at {x}, {y}");
                    queens++;
                }
                else
                {
                    Console.WriteLine("You can't put Queen here!");
                }
            }
            Console.WriteLine("Your board:");
            Console.WriteLine(brd);
            int conflicts = brd.CountConflicts();
            Console.WriteLine($"Found {conflicts} conclicts");

            Node root = new Node(brd);
            LDFS ldfs = new LDFS(root, size);
            Console.WriteLine(ldfs);
            // Console.WriteLine("Generated root node for this board: ");
            // Node root = new Node(brd);
            // Node.Expand(root);
            // int num = 0;
            // foreach (var node in root.successors)
            // {
            //     Console.WriteLine($"{num++}");
            //     node.state.DrawBoard();                
            // }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}