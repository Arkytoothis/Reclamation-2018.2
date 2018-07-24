using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class Helper
{
    public static TValue RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        List<TValue> values = Enumerable.ToList(dict.Values);

        int size = dict.Count;

        return values[Random.Range(0, size)];
    }

    public static TKey RandomKey<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        List<TKey> keys = Enumerable.ToList(dict.Keys);

        int size = dict.Count;

        return keys[Random.Range(0, size)];
    }
}