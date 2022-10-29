namespace Lab2;

internal class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Console.Write("Enter the size of board: ");
            int size = Int32.Parse(Console.ReadLine());
            Console.WriteLine();
            string input = "x";
            while (input != "y" && input != "n")
            {
                Console.Write("Do you want to limit the search? [y/n] ");
                input = Console.ReadLine();
            }
            bool choice = (input == "y");
            Board brd = new Board(size);
            Console.WriteLine("Your board:");
            Console.WriteLine(brd);
            Console.WriteLine($"{brd.CountConfs()} conflicts on the board");
            Node root = new Node(brd);
            int algoChoice = -1;
            while (algoChoice != 0 && algoChoice != 1)
            {
                Console.WriteLine("Choose LDFS or A*? [0/1]: ");
                algoChoice = Int32.Parse(Console.ReadLine());
            }

            if (algoChoice == 0)
            {
                LDFS ldfs = new LDFS(root, size, choice);
                Console.WriteLine(ldfs.DisplayResults());
            }
            else
            {
                AStar aStar = new AStar(root, choice);
                Console.WriteLine(aStar.DisplayResults());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}