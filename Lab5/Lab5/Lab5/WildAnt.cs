namespace Lab5;

public class WildAnt : Ant
{
    public override int pheromoneCoeff => 1;

    public override void Traverse(AntColony antColony)
    {
        while (true)
        {
            List<int> adjacents = GetAvailableVertices(antColony);
            if (adjacents.Count == 0) break;
            Random rng = new Random();
            int chosenVertice = rng.Next(0, adjacents.Count);

            MoveToVertice(antColony, adjacents, chosenVertice);
        }
    }
}