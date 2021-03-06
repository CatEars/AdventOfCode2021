using System.Collections.Generic;

namespace AdventOfCode2021.Util
{
    public static class DictionaryUtil
    {

        public static void AddOrSet<T>(this Dictionary<T, long> collection, T key, long num)
        {
            if (collection.ContainsKey(key))
            {
                collection[key] += num;
            }
            else
            {
                collection[key] = num;
            }
        }

        public static void AddOrSet<T>(this Dictionary<T, int> collection, T key, int num)
        {
            if (collection.ContainsKey(key))
            {
                collection[key] += num;
            }
            else
            {
                collection[key] = num;
            }
        }

        public static void Append<T, T2>(this Dictionary<T, List<T2>> collection, T key, T2 value)
        {
            if (collection.ContainsKey(key))
            {
                collection[key].Add(value);
            }
            else
            {
                collection[key] = new List<T2>() { value };
            }
        }

        public static int GetOrZero<T>(this Dictionary<T, int> collection, T key)
        {
            return collection.ContainsKey(key) ? collection[key] : 0;
        }

        public static long GetOrZero<T>(this Dictionary<T, long> collection, T key)
        {
            return collection.ContainsKey(key) ? collection[key] : 0L;
        }

        public static Dictionary<T2, T> Reverse<T, T2>(this Dictionary<T, T2> collection)
        {
            var result = new Dictionary<T2, T>();
            foreach (var (key, value) in collection)
            {
                result[value] = key;
            }
            return result;
        }

    }
}
