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

        public static int GetOrZero<T>(this Dictionary<T, int> collection, T key)
        {
            return collection.ContainsKey(key) ? collection[key] : 0;
        }

        public static long GetOrZero<T>(this Dictionary<T, long> collection, T key)
        {
            return collection.ContainsKey(key) ? collection[key] : 0L;
        }

    }
}
