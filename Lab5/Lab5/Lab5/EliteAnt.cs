namespace Lab5;

public class EliteAnt : Ant
{
    public override int pheromoneCoeff => 2;

    public override void Traverse(AntColony antColony)
    {
        List<int> adjacents = new List<int>() {0};
        while (adjacents.Count > 0) {
            adjacents = GetAvailableVertices(antColony);
            if (adjacents.Count == 0) return;
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