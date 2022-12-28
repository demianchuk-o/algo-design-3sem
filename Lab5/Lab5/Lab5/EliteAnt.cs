namespace Lab5;

public class EliteAnt : Ant
{
    protected override int pheromoneCoeff => 2;

    protected override void Traverse(AntColony antColony)
    {
        while (_currentLength < Graph._amtOfVertices)
        {
            List<int> adjacents = GetAvailableVertices(antColony);

            double[] probabilites = antColony.GetChoiceProbs(_path[_currentLength - 1], adjacents);

            int chosenVertice = 0;
            double maxProbability = 0d;

            while (chosenVertice < probabilites.Length && maxProbability < probabilites[chosenVertice])
            {
                maxProbability = probabilites[chosenVertice];
                chosenVertice++;
            }

            MoveToVertice(antColony, adjacents, chosenVertice);
        }
    }
}