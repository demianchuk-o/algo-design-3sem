using System.Net.Security;

namespace Lab5;

public abstract class Ant
{
    public int[] _path { get; }
    public int _currentLength;
    public int _pathDistance;
    public abstract int pheromoneCoeff { get; }
    public double[] pheromones { get; }

    public Ant()
    {
        _path = new int[Graph._amtOfVertices];
        pheromones = new double[Graph._amtOfVertices - 1];
        _currentLength = 0;
        _pathDistance = 0;
    }

    protected void SetPheromone(AntColony antColony)
    {
        pheromones[_currentLength - 2] = (double)antColony.Lmin / _pathDistance;
    }

    public void PlaceAtStart(int vertice)
    {
        _path[0] = vertice;
        _currentLength++;
    }
    
    public abstract void Traverse(AntColony antColony);

    protected List<int> GetAvailableVertices(AntColony antColony)
    {
        List<int> adjacents = antColony.graph.GetAdjacentVertices(_path[_currentLength - 1]);
        for (int i = 0; i < _currentLength; i++)
            adjacents.Remove(_path[i]);
        return adjacents;
    }

    protected void MoveToVertice(AntColony antColony, List<int> adjacents, int chosenVertice)
    {
        _path[_currentLength++] = adjacents[chosenVertice];
        _pathDistance += antColony.graph.DistanceMatrix[_path[_currentLength - 2], _path[_currentLength - 1]];
            
        SetPheromone(antColony);
    }

    public bool IsPathValid(Graph graph)
    {
        List<int> verts = graph.GetAdjacentVertices(_path[_currentLength - 1]);
        return _currentLength == Graph._amtOfVertices && verts.Contains(_path[0]);
    }
}