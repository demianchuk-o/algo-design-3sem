namespace Lab5;

public class RegularAnt : Ant
{
    public override int pheromoneCoeff => 1;

    public override void Traverse(AntColony antColony)
    {
        List<int> adjacents = new List<int>() {0};
        while (adjacents.Count > 0)
        {
            adjacents = GetAvailableVertices(antColony);
            if (adjacents.Count == 0) return;
            double[] probabilites = antColony.GetChoiceProbs(_path[_currentLength - 1], adjacents);

            for (int i = 1; i < probabilites.Length; i++)
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