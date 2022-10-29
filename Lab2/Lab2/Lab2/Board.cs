namespace Lab2;

public class Board
{
    private byte[] field;
    public Board(int _size = 8)
    {
        if (_size < 4 || _size > 100)
        {
            throw new ArgumentException();
        }
        field = new byte[_size];
        Random rng = new Random();
        for (int i = 0; i < _size; i++)
        {
            field[i] = (byte)rng.Next(0, _size);
        }
    }
    
    public Board(int otherSize, Board other)
    {
        field = new byte[other.field.Length];
        Array.Copy(other.field, field, other.field.Length);
    }

    public int GetSize() => field.Length;
    public byte GetRow(int col) => field[col];

    public override string ToString()
    {
        string board = String.Empty;
        foreach (var tile in field)
        {
            board += $"{tile} ";
        }

        board += "\n";
        board += "   ";
        for (int i = 0; i < field.Length; i++)
        {
            board += $" {i} ";
        }
        board += "\n";
        for (int i = 0; i < field.Length; i++)
        {
            board += $" {i} ";
            for (int j = 0; j < field.Length; j++)
            {
                if (i == field[j])
                {
                    board += "[Q]";
                }
                else
                {
                    board += "[ ]";
                }
            }
            board += "\n";
        }

        return board;
    }
    
    public int CountConfs()
    {
        int res = 0;
        for (int i = 0; i < field.Length; i++)
        {
            bool foundOnRow = false;
            bool foundOnMainDiag = false;
            bool foundOnSideDiag = false;
            for (int a = i+1; a < field.Length; a++)
            {
                if (!foundOnRow && field[i] == field[a])
                {
                    foundOnRow = true;
                    res++;
                }

                if (!foundOnMainDiag && i - field[i] == a - field[a])
                {
                    foundOnMainDiag = true;
                    res++;
                }

                if (!foundOnSideDiag && i + field[i] == a + field[a])
                {
                    foundOnSideDiag = true;
                    res++;
                }
            }
        }

        return res;
    }

    public void MoveQueen(int col, byte destination)
    {
        if (destination > field.Length - 1)
            throw new IndexOutOfRangeException();
        if (destination == field[col]) 
            return;

        field[col] = destination;
    }
}

