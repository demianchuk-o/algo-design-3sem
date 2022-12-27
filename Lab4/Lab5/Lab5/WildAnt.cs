namespace Lab5;

public class WildAnt : Ant
{
    protected override int pheromoneCoeff => 1;

    protected override void Traverse(AntColony antColony)
    {
        List<int> adjacents = GetAvailableVertices(antColony);

        Random rng = new Random();
        int chosenVertice = rng.Next(0, adjacents.Count);
        

        MoveToVertice(antColony, adjacents, chosenVertice);
    }
}