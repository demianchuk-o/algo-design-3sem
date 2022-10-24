using Microsoft.VisualBasic.CompilerServices;

namespace Lab2;

internal class Board
{
    private bool[][] field;
    private int counter;
    public int size { get; init; }
    public Board(int _size = 8)
    {
        if (_size < 4 || _size > 100)
        {
            throw new ArgumentException();
        }
        size = _size;
        counter = 0;
        field = new bool[size][];
        for (int i = 0; i < size; i++)
        {
            field[i] = new bool[size];
        }
    }

    public Board(ref Board other)
    {
        size = other.size;
        counter = other.counter;
        field = new bool[size][];
        for (int i = 0; i < size; i++)
        {
            field[i] = new bool[size];
            Array.Copy(other.field[i], field[i], size);
        }
    }

    public bool AddQueen(int row, int col)
    {
        if (row < 0 || row > size - 1 || col < 0 || col > size - 1)
            throw new ArgumentOutOfRangeException();
        if (counter > size)
            throw new OverflowException();
        if (ColContainsQueens(col))
            return false;
        if (field[row][col])
            return false;

        field[row][col] = true;
        counter++;
        return true;
    }

    private bool ColContainsQueens(int col)
    {
        for (int i = 0; i < size; i++)
        {
            if (field[i][col]) return true;
        }

        return false;
    }
    public void DrawBoard()
    {
        Console.Write("   ");
        for (int i = 0; i < size; i++)
        {
            Console.Write($" {i} ");
        }
        Console.WriteLine();
        for (int i = 0; i < size; i++)
        {
            Console.Write($" {i} ");
            for (int j = 0; j < size; j++)
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

    public int CountConflicts()
    {
        int conflicts = 0;
        for (int i = 0; i < size; i++)
        {
            conflicts += Horizontal(i);
            conflicts += Vertical(i);
        }

        for (int i = 0; i < size - 1; i++)
        {
            conflicts += LeftTopRightBottom(0, i);
        }

        for (int i = 1; i < size - 1; i++)
        {
            conflicts += LeftTopRightBottom(i, 0);
        }

        for (int i = 1; i < size - 1; i++)
        {
            conflicts += LeftBottomRightTop(i, 0);
        }

        for (int i = 1; i < size - 1; i++)
        {
            conflicts += LeftBottomRightTop(size - 1, i);
        }
        return conflicts;
    }

    private int Horizontal(int row)
    {
        int result = -1;
        for (int i = 0; i < size; i++)
        {
            if (field[row][i]) result++;
        }

        return (result < 0) ? 0 : result;
    }

    private int Vertical(int col)
    {
        int result = -1;
        for (int i = 0; i < size; i++)
        {
            if (field[i][col]) result++;
        }

        return (result < 0) ? 0 : result;
    }

    private int LeftTopRightBottom(int row, int col)
    {
        int result = -1;
        while (row < size && col < size)
        {
            if (field[row++][col++]) result++;
        }
        
        return (result < 0) ? 0 : result;
    }

    private int LeftBottomRightTop(int row, int col)
    {
        int result = -1;
        while (row >= 0 && col < size)
        {
            if (field[row--][col++]) result++;
        }

        return (result < 0) ? 0 : result;
    }

    public void MoveQueen(int row, int col, int destination)
    {
        if (!field[row][col])
            throw new Exception("No Queen at specified position");
        if (destination < 0 || destination > size - 1)
            throw new IndexOutOfRangeException();
        if (destination == col) 
            return;

        field[row][col] = false;
        field[destination][col] = true;
    }
}

