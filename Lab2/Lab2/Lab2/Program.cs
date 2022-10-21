namespace Lab2;

internal class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Board brd = new Board();
            int x, y;
            int queens = 0;
            while(queens < 8)
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
                    Console.WriteLine("You can't put another Queen here!");
                }
            }
            Console.WriteLine("Your board:");
            brd.DrawBoard();
            brd.FindConflicts();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}