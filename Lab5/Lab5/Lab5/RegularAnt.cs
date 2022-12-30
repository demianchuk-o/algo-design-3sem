namespace Lab5;

public class RegularAnt : Ant
{
    public override int pheromoneCoeff => 1;

    public override void Traverse(AntColony antColony)
    {
        while (true)
        {
            List<int> adjacents = GetAvailableVertices(antColony);
            if (adjacents.Count == 0) return;
            double[] probabilites = antColony.GetChoiceProbs(_path[_currentLength - 1], adjacents);
            Random rng = new Random();
            double randomChoice = rng.NextDouble();
            int chosenVertice = 0;
            if (probabilites.Length > 1)
            {
                for (int i = 1; i < probabilites.Length; i++)
                    probabilites[i] += probabilites[i - 1];
                
                while (chosenVertice < probabilites.Length && randomChoice > probabilites[chosenVertice])
                    chosenVertice++;
            }

            MoveToVertice(antColony, adjacents, chosenVertice);
        } 
    }
}