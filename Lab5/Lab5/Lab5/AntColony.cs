using System.Reflection.Metadata;

namespace Lab5;

public class AntColony
{
    public Graph graph;
    public double _alpha;
    public double _beta;
    public double _rho;
    public int Lmin;
    private bool _differentPlacement;
    private int _elites;
    private int _regs;
    private int _wilds;
    private Ant[] colony;
    private double[,] _pheromonesMatrix;
    private int[] AllTimeBestPath;
    private int AllTimeBestDistance;
    public static string filepath = "data.txt";

    public AntColony(Graph g, double a, double b, double r, int lm, bool placement, int elite, int reg, int wild)
    {
        graph = g;
        _alpha = a;
        _beta = b;
        _rho = r;
        Lmin = lm;
        _differentPlacement = placement;
        _elites = elite;
        _regs = reg;
        _wilds = wild;

        _pheromonesMatrix = new double[Graph._amtOfVertices, Graph._amtOfVertices];
        for (int i = 0; i < Graph._amtOfVertices; i++)
        {
            for (int j = 0; j < Graph._amtOfVertices; j++)
            {
                if(graph.DistanceMatrix[i,j]>0)
                    _pheromonesMatrix[i, j] = 0.1;
                else
                    _pheromonesMatrix[i, i] = 0d;
            }
        }
        
        AllTimeBestDistance = Int32.MaxValue;
    }

    public void Start(int iterations)
    {
        int itr = 0;
        int[] bestPath = new int[Graph._amtOfVertices];
        int bestDistance;
        while (itr < iterations)
        {
            itr++;
            CreateColony();
            PlaceAnts();
            foreach (Ant ant in colony)
                ant.Traverse(this);
            bestDistance = Int32.MaxValue;
            foreach (Ant ant in colony)
            {
                if (ant.IsPathValid(graph) && bestDistance > ant._pathDistance)
                {
                    bestDistance = ant._pathDistance;
                    bestPath = ant._path;
                }
            }

            if (AllTimeBestDistance > bestDistance)
            {
                AllTimeBestDistance = bestDistance;
                AllTimeBestPath = bestPath;
            }
            PlacePheromones();
            
        }
    }

    private void CreateColony()
    {
        colony = new Ant[_elites + _regs + _wilds];
        int offset = 0;
        if (_elites > 0)
        {
            for (int i = 0; i < _elites; i++)
                colony[i + offset] = new EliteAnt();
            offset += _elites;
        }

        if (_regs > 0)
        {
            for (int i = 0; i < _regs; i++)
                colony[i + offset] = new RegularAnt();
            offset += _regs;
        }

        for (int i = 0; i < _wilds; i++)
            colony[i + offset] = new WildAnt();
    }
    public double[] GetChoiceProbs(int vertice, List<int> availableVertices)
    {
        double[] probs = new double[availableVertices.Count];
        double sumOfProbs = 0d; 
        for (int i = 0; i < probs.Length; i++)
        {
            probs[i] = CalculateProbability(vertice, availableVertices[i]);
            sumOfProbs += probs[i];
        }

        for (int i = 0; i < probs.Length; i++)
            probs[i] /= sumOfProbs;

        return probs;
    }

    private double CalculateProbability(int vertice, int probable) =>
        Math.Pow(_pheromonesMatrix[vertice, probable], _alpha) * Math.Pow(getVisibility(vertice, probable), _beta);

    double getVisibility(int i, int j) => 1d / graph.DistanceMatrix[i, j];
    
    private void PlaceAnts()
    {
        Random rng = new Random();
        if (_differentPlacement)
        {
            foreach (Ant ant in colony)
                ant.PlaceAtStart(rng.Next(0,300));
            return;
        }

        int vertice = rng.Next(0, 300);
        foreach (Ant ant in colony)
            ant.PlaceAtStart(vertice);
    }

    private void PlacePheromones()
    {
        for (int i = 0; i < _pheromonesMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < _pheromonesMatrix.GetLength(1); j++)
            {
                _pheromonesMatrix[i, j] *= (1d - _rho);
            }
        }
        foreach (Ant ant in colony)
        {
            for (int i = 1; i < ant._currentLength; i++)
            {
                _pheromonesMatrix[ant._path[i - 1], ant._path[i]] += ant.pheromones[i - 1] * ant.pheromoneCoeff;
            }
        }
    }

    public void WritePath()
    {
        for (int i = 0; i < AllTimeBestPath.Length; i++)
            Console.Write($"{AllTimeBestPath[i]} ");
        Console.WriteLine($"\nwith a distance of {AllTimeBestDistance}");
    }

    public void SaveResults()
    {
        StreamWriter writer = new StreamWriter(AntColony.filepath, false);
        string diffplace = this._differentPlacement ? "yes" : "no";
        string message = $"Alpha: {_alpha}\nBeta: {_beta}\nRho: {_rho}\nL min: {Lmin}\nAnts:\n\tElite: {_elites}\n\tRegular: {_regs}\n\tWild: {_wilds}\nDifferent Placement: {diffplace}";
        writer.Write(message);
        message = "\nPheromones matrix:\n";
        for (int i = 0; i < _pheromonesMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < _pheromonesMatrix.GetLength(1); j++)
                message += $"{_pheromonesMatrix[i, j]} ";
            
            message += "\n";
        }
        writer.Write(message);
        message = "\nThe best path found:\n";
        for (int i = 0; i < AllTimeBestPath.Length; i++)
            message += $"{AllTimeBestPath[i]} ";

        message += $"\nwith a distance of {AllTimeBestDistance}";
        writer.Write(message);

        writer.Dispose();
    }
}