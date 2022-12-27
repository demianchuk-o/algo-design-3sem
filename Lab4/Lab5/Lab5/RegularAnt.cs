namespace Lab5;

public class RegularAnt : Ant
{
    protected override int pheromoneCoeff => 1;

    protected override void Traverse(AntColony antColony)
    {
        while (_currentLength < Graph._amtOfVertices)
        {
            List<int> adjacents = GetAvailableVertices(antColony);

            double[] probabilites = antColony.GetChoiceProbs(_path[_currentLength - 1], adjacents);
            
            for (int i = 1; i <= probabilites.Length; i++)
                probabilites[i] += probabilites[i - 1];
            
            Random rng = new Random();
            double randomChoice = rng.NextDouble();
            int chosenVertice = 0;
            while (chosenVertice < probabilites.Length && randomChoice > probabilites[chosenVertice])
                chosenVertice++;

            MoveToVertice(antColony, adjacents, chosenVertice);
        }
    }
}