namespace Lab2;

internal class Board
{
    private bool[][] field;
    private int counter;
    
    public Board()
    {
        field = new bool[8][];
        for (int i = 0; i < 8; i++)
        {
            field[i] = new bool[8];
            for (int j = 0; j < 8; j++)
            {
                field[i][j] = false;
            }
        }

        counter = 0;
    }

    public bool AddQueen(int x, int y)
    {
        if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
        {
            if (counter <= 8)
            {
                if (!field[x][y])
                {
                    field[x][y] = true;
                    counter++;
                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public void DrawBoard()
    {
        for (int i = 0; i < field.Length; i++)
        {
            for (int j = 0; j < field[i].Length; j++)
            {
                if (field[i][j])
                {
                    Console.Write("[Q]");
                }
                else
                {
                    Console.Write("[ ]");
                }
            }
            Console.WriteLine();
        }
    }

    public void FindConflicts()
    {
        for (int i = 0; i < field.Length; i++)
        {
            for (int j = 0; j < field[i].Length; j++)
            {
                if (field[i][j])
                {
                    //Horizontal search
                    for (int k = 0; k < field.Length; k++)
                    {
                        if (field[i][k] && k != j)
                        {
                            Console.WriteLine($"{i}, {j} conflicts with {i}, {k})");
                        }
                    }
                    //Vertical search
                    for (int k = 0; k < field.Length; k++)
                    {
                        if (field[k][j] && k != i)
                        {
                            Console.WriteLine($"{i}, {j} conflicts with {k}, {j}");
                        }
                    }
                    //LT-RB search
                    int delta = j - i;
                    int start = (delta >= 0) ? 0 : delta;
                    int end = (delta >= 0) ? 7 - delta : 7;
                    for (int k = start; k <= end; k++)
                    {
                        if (delta >= 0)
                        {
                            if (field[k][k + delta] && k != i && k + delta != j)
                            {
                                Console.WriteLine($"{i}, {j} conflicts with {k}, {k+delta}");
                            }
                        }
                        else
                        {
                            if (field[k][k - delta] && k != i && k - delta != j)
                            {
                                Console.WriteLine($"{i}, {j} conflicts with {k}, {k+delta}");
                            }
                        }
                    }
                    
                    //LB-RT search
                    delta = j + i;
                    start = (delta <= 7) ? delta : 7;
                    end = (delta <= 7) ? 0 : delta - 7;
                    for (int k = start; k >= end; k--)
                    {
                        if (field[k][delta - k] && k != i && delta - k != j)
                            {
                                Console.WriteLine($"{i}, {j} conflicts with {k}, {delta - k}");
                            }
                        }
                }
            }
        }
    }
}
