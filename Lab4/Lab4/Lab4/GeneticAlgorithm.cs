namespace Lab4;

public class GeneticAlgorithm
{
    private Store _store;
    private int[][] _population;
    private static int _populationSize = 100;
    private static int _capacity = 150;
    private static double _mutationProb = 0.05;


    public GeneticAlgorithm()
    {
        _store = new Store();
        _population = new int[_populationSize][];
        int[] randomItems = new int[Store.AMT_OF_ITEMS];
        for (int i = 0; i < _populationSize; i++)
        {
            randomItems[i] = i;
            _population[i] = new int[_populationSize];
            for (int j = 0; j < _populationSize; j++)
                _population[i][j] = 0;
        }
        new Random().Shuffle(randomItems);

        for (int i = 0; i < _populationSize; i++)
        {
            _population[i][randomItems[i]] = 1;
        }
    }
}
/*
 Задача про рюкзак 
 (місткість P=150, 100 предметів,                                   +
 цінність предметів від 2 до 10 (випадкова),                        +
 вага від 1 до 5 (випадкова)),                                      +
 генетичний алгоритм 
 (початкова популяція 100 осіб кожна по 1 різному предмету,         +
 оператор схрещування рівномірний, 
 мутація з ймовірністю 5% два випадкові гени міняються місцями). 
 Розробити власний оператор локального покращення.
 */