namespace Lab4;

public class Store
{
    public static int AMT_OF_ITEMS = 100;
    public static int VALUE_LOWER = 2;
    public static int VALUE_UPPER = 10;
    public static int WEIGHT_LOWER = 1;
    public static int WEIGHT_UPPER = 5;
    public readonly Tuple<int, int>[] ITEMS;

    public Store()
    {
        Random rng = new Random();
        for (int i = 0; i < AMT_OF_ITEMS; i++)
        {
            ITEMS[i] = new Tuple<int, int>(rng.Next(VALUE_LOWER, VALUE_UPPER), rng.Next(WEIGHT_LOWER, WEIGHT_UPPER));
        }
    }
}