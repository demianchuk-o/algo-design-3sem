namespace Lab4;

public class GeneticAlgorithm
{
    private Store _store;
    private bool[][] _population;
    private static int _populationSize = 100;
    private static int _chromosomeSize = 100;
    private static int _capacity = 150;
    private static double _mutationProb = 0.05;


    public GeneticAlgorithm()
    {
        _store = new Store();
        _population = new bool[_populationSize][];
        
    }

    public void Start(int iterations)
    {
        SetStartPopulation();
        int bestItemIndex = 0;
        int itr = 0;
        int best = 0;
        while (itr < iterations)
        {
            itr++;
            best = 0;
            int worst = 0;
            GetExtremeIndices(ref best, ref worst);
            Random rng = new Random();
            int secondRandom = best;
            while (secondRandom == best)
                secondRandom = rng.Next(0, _populationSize);

            bool[] successor = UniformCrossover(best, secondRandom);
            double mutationRand = rng.NextDouble();
            bool[] mutant = new bool[_chromosomeSize];
            if (mutationRand < _mutationProb)
            {
                mutant = Mutate(successor);
                if(GetTotalWeight(mutant) <= _capacity)
                    Array.Copy(mutant, successor, _chromosomeSize);
            }

            bestItemIndex = GetBestItemIndex(successor);
            //if(successor[bestItemIndex] == false)
            //    successor[bestItemIndex] = !successor[bestItemIndex];
            successor[bestItemIndex] = !successor[bestItemIndex];

            
            
            if(GetTotalWeight(successor) <= _capacity)
                Array.Copy(successor, _population[worst], _chromosomeSize);
        }
        Console.WriteLine("Item store:");
        Console.Write("Value:\t");
        for (int i = 0; i < Store.AMT_OF_ITEMS; i++)
        {
            Console.Write($"{_store.Items[i].Item1} ");
        }
        Console.Write("\nWeight:\t");
        for (int i = 0; i < Store.AMT_OF_ITEMS; i++)
        {
            Console.Write($"{_store.Items[i].Item2} ");
        }

        PrintResults(iterations, best);

    }

    private void PrintResults(int iterations, int best)
    {
        Console.WriteLine($"\nThe best chromosome found in {iterations} iterations: ");
        int toNum = -1;
        for (int i = 0; i < _chromosomeSize; i++)
        {
            toNum = (_population[best][i]) ? 1 : 0;
            Console.Write($"{toNum} "); 
        }

        int bestValue = 0;
        int bestWeight = GetTotalWeight(_population[best]);

        for (int i = 0; i < _chromosomeSize; i++)
        {
            if (_population[best][i])
            {
                bestValue += _store.Items[i].Item1;
            }
        }
        Console.WriteLine($"\nIt weights {bestWeight} and has value of {bestValue}");
    }

    private void SetStartPopulation()
    {
        int[] randomItems = new int[Store.AMT_OF_ITEMS];
        for (int i = 0; i < _populationSize; i++)
        {
            randomItems[i] = i;
            _population[i] = new bool[_chromosomeSize];
            for (int j = 0; j < _chromosomeSize; j++)
                _population[i][j] = false;
        }
        new Random().Shuffle(randomItems);

        for (int i = 0; i < _populationSize; i++)
        {
            _population[i][randomItems[i]] = true;
        }
    }

    private void GetExtremeIndices(ref int best, ref int worst)
    {
        int min = Int32.MaxValue;
        int max = 0;
        for (int i = 0; i < _populationSize; i++)
        {
            int totalValue = 0;
            for (int j = 0; j < _chromosomeSize; j++)
            {
                if(_population[i][j])
                {
                    totalValue += _store.Items[j].Item1;
                }
            }

            if (min > totalValue)
            {
                min = totalValue;
                worst = i;
            }

            if (max < totalValue)
            {
                max = totalValue;
                best = i;
            }
        }
    }

    private bool[] UniformCrossover(int first, int second)
    {
        Random rng = new Random();
        int choice = -1;
        bool[] crossover = new bool[_chromosomeSize];
        for (int i = 0; i < _chromosomeSize; i++)
        {
            choice = rng.Next(0, 2);
            if (choice == 0)
                crossover[i] = _population[first][i];
            else
                crossover[i] = _population[second][i];
        }

        return crossover;
    }

    private bool[] Mutate(bool[] successor)
    {
        bool[] mutant = new bool[_chromosomeSize];
        Array.Copy(successor, mutant, _chromosomeSize);
        Random rng = new Random();
        int first = rng.Next(0, _chromosomeSize);
        int second = first;
        while (second == first)
            second = rng.Next(0, _chromosomeSize);

        (mutant[first], mutant[second]) = (mutant[second], mutant[first]);
        return mutant;
    }

    private int GetTotalWeight(bool[] chromosome)
    {
        int totalWeight = 0;
        for (int i = 0; i < _chromosomeSize; i++)
        {
            if (chromosome[i] == true)
                totalWeight += _store.Items[i].Item2;
        }

        return totalWeight;
    }

    private int GetBestItemIndex(bool[] chromosome)
    {
        int index = 0;
        double bestValue = 0d;

        for (int i = 0; i < Store.AMT_OF_ITEMS; i++)
        {
            double possibleVal = (double)_store.Items[i].Item1 / _store.Items[i].Item2;
            if (!chromosome[i] && bestValue < possibleVal)
            {
                bestValue = possibleVal;
                index = i;
            }
        }
        
        return index;
    }
    
}
