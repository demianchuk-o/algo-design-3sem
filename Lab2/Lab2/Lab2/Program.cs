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
            Console.WriteLine("Your board:");
            Console.WriteLine(brd);
            Console.WriteLine(brd.CountConfs());

            Node root = new Node(brd);
            LDFS ldfs = new LDFS(root, size - 1);
            Console.WriteLine(ldfs);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}