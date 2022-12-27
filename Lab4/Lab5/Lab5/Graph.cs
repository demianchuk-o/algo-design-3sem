namespace Lab5;

public class Graph
{
    public static int _amtOfVertices = 300;
    public static int _verticeDegreeMax = 30;
    public static int _minDistance = 5;
    public static int _maxDistance = 150;
    public int[,] DistanceMatrix { get; }

    public Graph()
    {
        DistanceMatrix = new int[_amtOfVertices, _amtOfVertices];
            
        Random rng = new Random();
        int edges;
        for (int i = 0; i < _amtOfVertices; i++)
        {
            edges = 0;
            for (int j = i; j < _amtOfVertices; j++)
            {
                if (j == i) DistanceMatrix[i,j] = int.MaxValue;
                else if (edges <= _verticeDegreeMax)
                {
                    if (rng.Next(_amtOfVertices) < _verticeDegreeMax)
                    {
                        DistanceMatrix[i,j] = rng.Next(_minDistance, _maxDistance + 1);
                        DistanceMatrix[j,i] = DistanceMatrix[i,j];
                        edges++;
                    }
                    else
                    {
                        DistanceMatrix[i,j] = -1;
                        DistanceMatrix[j,i] = -1;
                    }
                }
            }
        }
    }

    public List<int> GetAdjacentVertices(int vertice)
    {
        List<int> adjacentVertices = new List<int>();
        for (int i = 0; i < _amtOfVertices; i++)
            if (vertice != i && DistanceMatrix[vertice, i] != -1) adjacentVertices.Add(i);

        return adjacentVertices;
    }

    public int GetCycleDistance(int[] cycle)
    {
        int distance = 0;
        for (int i = 1; i <= cycle.Length; i++)
        {
            distance += DistanceMatrix[i - 1, i];
        }

        return distance;
    }
}