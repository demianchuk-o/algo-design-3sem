namespace Lab5;

public abstract class Ant
{
    public int[] _path { get; private set; }
    private int _currentLength;
    private int _pathDistance;
    protected abstract int pheromoneCoeff { get; }
    public double[] pheromones { get; private set; }

    public Ant()
    {
        _path = new int[Graph._amtOfVertices];
        pheromones = new double[Graph._amtOfVertices - 1];
        _currentLength = 0;
        _pathDistance = 0;
    }

    protected void SetPheromone(AntColony ac)
    {
        pheromones[_currentLength - 1] = (double)ac._Lmin / _pathDistance;
    }

    protected void PlaceAtStart(int vertice)
    {
        _path[0] = vertice;
        _currentLength++;
    }
    
    protected abstract void Traverse(AntColony antColony, Graph graph);
}