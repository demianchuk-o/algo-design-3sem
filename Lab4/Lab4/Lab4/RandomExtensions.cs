namespace Lab4;

static class RandomExtensions
{
    //Fisher-Yates Algorithm for shuffling an array
    public static void Shuffle<T> (this Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = rng.Next(n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
}