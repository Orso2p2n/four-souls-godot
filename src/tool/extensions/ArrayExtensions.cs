using Godot;
using Godot.Collections;

public static class ArrayExtensions
{
    public static void Shuffle<[MustBeVariant] T>(this Array<T> array, RandomNumberGenerator rng)
    {
        int n = array.Count;
        while (n > 1)
        {
            n--;
            int k = rng.RandiRange(0, n);
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }
}