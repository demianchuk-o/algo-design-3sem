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
            brd.DrawBoard();
            int conflicts = brd.CountConflicts();
            Console.WriteLine($"Found {conflicts} conclicts");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}