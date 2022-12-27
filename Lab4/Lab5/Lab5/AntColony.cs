namespace Lab5;

public class AntColony
{
     public Graph graph;
     public double _alpha;
     public double _beta;
     public double _rho;
     public int _Lmin;
     private Ant[] colony;
     //private double[,] _visibilityMatrix;
     private double[,] _pheromonesMatrix;

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
          {
               probs[i] = probs[i] / sumOfProbs;
          }
          
          
          return probs;
     }

     private double CalculateProbability(int vertice, int probable) =>
          Math.Pow(_pheromonesMatrix[vertice, probable], _alpha) * Math.Pow(getVisibility(vertice, probable), _beta);

     double getVisibility(int i, int j) => 1d / graph.DistanceMatrix[i, j];
}