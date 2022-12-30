namespace Lab5;

public class WildAnt : Ant
{
    public override int pheromoneCoeff => 1;

    public override void Traverse(AntColony antColony)
    {
        List<int> adjacents = new List<int>() {0};
        while (adjacents.Count > 0)
        {
            adjacents = GetAvailableVertices(antColony);
            if (adjacents.Count == 0) return;
            Random rng = new Random();
            int chosenVertice = rng.Next(0, adjacents.Count);

            MoveToVertice(antColony, adjacents, chosenVertice);
        }
    }
}