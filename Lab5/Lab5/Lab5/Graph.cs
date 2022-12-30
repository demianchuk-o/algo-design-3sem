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
        int i = 0;
        int j = 0;
        int weight = 0;
        StreamReader reader = new StreamReader(path);
        string? line = reader.ReadLine();
        while (line != null)
        {
            string[] split = line.Split(',');
            i = Int32.Parse(split[0]);
            weight = Int32.Parse(split[1]);
            j = Int32.Parse(split[2]);
            DistanceMatrix[i, j] = weight;
            DistanceMatrix[j, i] = weight;
            line = reader.ReadLine();
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