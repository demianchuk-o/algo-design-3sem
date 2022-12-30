namespace Lab5;

public class Graph
{
    private static string path = "graph.txt";
    public static int _amtOfVertices = 300;
    public int[,] DistanceMatrix { get; }

    public Graph()
    {
        DistanceMatrix = new int[_amtOfVertices, _amtOfVertices];
        for (int k = 0; k < _amtOfVertices; k++)
        {
            for (int l = 0; l < _amtOfVertices; l++)
            {
                DistanceMatrix[k, l] = -1;
            }
            DistanceMatrix[k, k] = 0;
        }

        int weight = 0;
        StreamReader reader = new StreamReader(path);
        for (int i = 0; i < _amtOfVertices; i++)
        {
            string[] line = reader.ReadLine().Split();
            for (int j = 0; j < _amtOfVertices; j++)
            {
                DistanceMatrix[i,j] = Int32.Parse(line[j]);
            }
        }
        reader.Dispose();
    }

    public List<int> GetAdjacentVertices(int vertice)
    {
        List<int> adjacentVertices = new List<int>();
        for (int i = 0; i < _amtOfVertices; i++)
            if (vertice != i && DistanceMatrix[vertice, i] != -1) adjacentVertices.Add(i);

        return adjacentVertices;
    }
    
}